#!/usr/bin/env node
import {
  cp,
  mkdir,
  readFile,
  rm,
  rename,
  stat,
  writeFile,
} from "node:fs/promises";
import { existsSync } from "node:fs";
import path from "node:path";
import process from "node:process";

const repoRoot = path.resolve(import.meta.dirname, "..");

const textFileExtensions = new Set([
  ".cs",
  ".csproj",
  ".css",
  ".editorconfig",
  ".html",
  ".json",
  ".md",
  ".mjs",
  ".props",
  ".sh",
  ".slnx",
  ".ts",
  ".tsx",
  ".xml",
  ".yaml",
  ".yml",
]);

const excludedPathSegments = new Set([
  ".git",
  ".pnpm-store",
  ".vs",
  "bin",
  "coverage",
  "dist",
  "node_modules",
  "obj",
  "playwright-report",
  "test-results",
]);

const excludedRelativePaths = new Set(["docs/template", "openspec/changes"]);

function usage() {
  console.log(
    `Usage: node scripts/bootstrap-template.mjs --product-name "Acme Desk" --output ../acme-desk`,
  );
}

function parseArgs(argv) {
  const args = {
    productName: "",
    output: "",
  };

  for (let i = 0; i < argv.length; i += 1) {
    const arg = argv[i];
    if (arg === "--") {
      continue;
    }

    if (arg === "--product-name") {
      args.productName = argv[++i] ?? "";
      continue;
    }

    if (arg === "--output") {
      args.output = argv[++i] ?? "";
      continue;
    }

    if (arg === "--help" || arg === "-h") {
      usage();
      process.exit(0);
    }

    throw new Error(`Unknown argument: ${arg}`);
  }

  return args;
}

function splitWords(productName) {
  const matches = productName
    .normalize("NFKD")
    .replace(/[\u0300-\u036f]/g, "")
    .match(/[A-Za-z0-9]+/g);

  if (!matches?.length) {
    throw new Error(
      "Product name must contain at least one ASCII letter or digit.",
    );
  }

  return matches;
}

function toPascalCase(words) {
  return words
    .map((word) => `${word[0].toUpperCase()}${word.slice(1).toLowerCase()}`)
    .join("");
}

function deriveNames(productName) {
  const words = splitWords(productName);
  const pascal = toPascalCase(words);
  const slug = words.map((word) => word.toLowerCase()).join("-");
  const snake = words.map((word) => word.toLowerCase()).join("_");
  const display = words
    .map((word) => `${word[0].toUpperCase()}${word.slice(1)}`)
    .join(" ");

  if (!/^[A-Za-z][A-Za-z0-9]*$/.test(pascal)) {
    throw new Error(
      "Product name must derive a namespace that starts with a letter.",
    );
  }

  return {
    display,
    npmScope: `@${slug}`,
    pascal,
    slug,
    snake,
  };
}

function getMappings(names) {
  return [
    ["@modular-template", names.npmScope],
    ["ModularTemplate", names.pascal],
    ["Modular Template", names.display],
    ["modular-template", names.slug],
    ["modular_template", names.snake],
    ["net-react-modular-template", names.slug],
  ];
}

function normalizeRelative(relativePath) {
  return relativePath.split(path.sep).join("/");
}

function shouldExclude(src) {
  const relative = normalizeRelative(path.relative(repoRoot, src));
  if (!relative) {
    return false;
  }

  if (excludedRelativePaths.has(relative)) {
    return true;
  }

  if (
    [...excludedRelativePaths].some((excluded) =>
      relative.startsWith(`${excluded}/`),
    )
  ) {
    return true;
  }

  return relative
    .split("/")
    .some((segment) => excludedPathSegments.has(segment));
}

function isTextFile(filePath) {
  const name = path.basename(filePath);
  if (name === ".gitignore" || name === ".gitattributes") {
    return true;
  }

  return textFileExtensions.has(path.extname(filePath));
}

async function walk(root) {
  const entries = await import("node:fs/promises").then((fs) =>
    fs.readdir(root, { withFileTypes: true }),
  );
  const results = [];

  for (const entry of entries) {
    const fullPath = path.join(root, entry.name);
    results.push(fullPath);
    if (entry.isDirectory()) {
      results.push(...(await walk(fullPath)));
    }
  }

  return results;
}

function applyMappings(value, mappings) {
  return mappings.reduce(
    (current, [from, to]) => current.split(from).join(to),
    value,
  );
}

async function rewriteFile(filePath, mappings) {
  if (!isTextFile(filePath)) {
    return;
  }

  const buffer = await readFile(filePath);
  if (buffer.includes(0)) {
    return;
  }

  const original = buffer.toString("utf8");
  const updated = applyMappings(original, mappings);
  if (updated !== original) {
    await writeFile(filePath, updated, "utf8");
  }
}

async function rewritePaths(outputRoot, mappings) {
  const entries = (await walk(outputRoot)).sort((a, b) => b.length - a.length);

  for (const currentPath of entries) {
    const parent = path.dirname(currentPath);
    const name = path.basename(currentPath);
    const rewrittenName = applyMappings(name, mappings);
    if (rewrittenName !== name) {
      await rename(currentPath, path.join(parent, rewrittenName));
    }
  }
}

async function removeTemplateDocReferences(outputRoot) {
  const docsReadme = path.join(outputRoot, "docs", "README.md");
  if (!existsSync(docsReadme)) {
    return;
  }

  const lines = (await readFile(docsReadme, "utf8")).split(/\r?\n/);
  const filtered = [];
  let skipContinuation = false;

  for (const line of lines) {
    if (line.startsWith("- ") && line.includes("(template/")) {
      skipContinuation = true;
      continue;
    }

    if (skipContinuation && line.startsWith("  ")) {
      continue;
    }

    skipContinuation = false;
    filtered.push(line);
  }

  await writeFile(docsReadme, `${filtered.join("\n").trimEnd()}\n`, "utf8");
}

async function removeTemplateAutomation(outputRoot) {
  await rm(path.join(outputRoot, "scripts", "bootstrap-template.mjs"), {
    force: true,
  });
  await rm(path.join(outputRoot, "scripts", "verify-bootstrap.mjs"), {
    force: true,
  });

  const packageJsonPath = path.join(outputRoot, "package.json");
  if (existsSync(packageJsonPath)) {
    const packageJson = JSON.parse(await readFile(packageJsonPath, "utf8"));
    delete packageJson.scripts?.["template:bootstrap"];
    delete packageJson.scripts?.["template:verify"];
    await writeFile(
      packageJsonPath,
      `${JSON.stringify(packageJson, null, 2)}\n`,
      "utf8",
    );
  }

  const scriptsReadme = path.join(outputRoot, "scripts", "README.md");
  if (existsSync(scriptsReadme)) {
    const lines = (await readFile(scriptsReadme, "utf8"))
      .split(/\r?\n/)
      .filter((line) => !line.includes("template rename"));
    await writeFile(scriptsReadme, `${lines.join("\n").trimEnd()}\n`, "utf8");
  }
}

async function updateRootReadme(outputRoot, names) {
  const readmePath = path.join(outputRoot, "README.md");
  if (!existsSync(readmePath)) {
    return;
  }

  const readme = `# ${names.slug}

${names.display} product repository bootstrapped from the .NET + React modular-monolith template.

Substantial runtime behavior starts from accepted OpenSpec artifacts or durable
architecture decisions. Stable governance, architecture, platform, testing, and
module guidance lives under \`docs/\`.

Start with:

- [docs/README.md](docs/README.md) for stable documentation.
- [docs/governance.md](docs/governance.md) for hard project rules.
- [docs/openspec.md](docs/openspec.md) for the spec-driven development
  workflow.
`;

  await writeFile(readmePath, readme, "utf8");
}

async function rewriteFiles(outputRoot, mappings) {
  const entries = await walk(outputRoot);
  for (const entry of entries) {
    const entryStat = await stat(entry);
    if (entryStat.isFile()) {
      await rewriteFile(entry, mappings);
    }
  }
}

async function bootstrap() {
  const args = parseArgs(process.argv.slice(2));
  if (!args.productName.trim()) {
    throw new Error("--product-name is required.");
  }

  if (!args.output.trim()) {
    throw new Error("--output is required.");
  }

  const outputRoot = path.resolve(args.output);
  if (existsSync(outputRoot)) {
    throw new Error(`Output path already exists: ${outputRoot}`);
  }

  const names = deriveNames(args.productName);
  const mappings = getMappings(names);

  await mkdir(path.dirname(outputRoot), { recursive: true });
  await cp(repoRoot, outputRoot, {
    dereference: false,
    errorOnExist: true,
    filter: (src) => !shouldExclude(src),
    force: false,
    recursive: true,
  });

  await rewriteFiles(outputRoot, mappings);
  await rewritePaths(outputRoot, mappings);
  await removeTemplateDocReferences(outputRoot);
  await removeTemplateAutomation(outputRoot);
  await updateRootReadme(outputRoot, names);

  console.log(`Created ${names.display} at ${outputRoot}`);
  console.log(`Namespace: ${names.pascal}`);
  console.log(`Slug: ${names.slug}`);
  console.log(`Database slug: ${names.snake}`);
  console.log(`NPM scope: ${names.npmScope}`);
}

bootstrap().catch((error) => {
  console.error(error.message);
  process.exit(1);
});
