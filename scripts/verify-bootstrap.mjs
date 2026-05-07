#!/usr/bin/env node
import { mkdtemp, readFile, rm, stat } from "node:fs/promises";
import { spawn } from "node:child_process";
import os from "node:os";
import path from "node:path";
import process from "node:process";

const repoRoot = path.resolve(import.meta.dirname, "..");

const knownPlaceholders = [
  "@modular-template",
  "ModularTemplate",
  "Modular Template",
  "modular-template",
  "modular_template",
  "net-react-modular-template",
];

const ignoredSegments = new Set([
  ".git",
  ".pnpm-store",
  "bin",
  "coverage",
  "dist",
  "node_modules",
  "obj",
  "playwright-report",
  "test-results",
]);

function parseArgs(argv) {
  const args = {
    full: false,
    keep: false,
    productName: "Acme Desk",
  };

  for (let i = 0; i < argv.length; i += 1) {
    const arg = argv[i];
    if (arg === "--") {
      continue;
    }

    if (arg === "--full") {
      args.full = true;
      continue;
    }

    if (arg === "--keep") {
      args.keep = true;
      continue;
    }

    if (arg === "--product-name") {
      args.productName = argv[++i] ?? "";
      continue;
    }

    if (arg === "--help" || arg === "-h") {
      console.log(
        'Usage: node scripts/verify-bootstrap.mjs [--product-name "Acme Desk"] [--full] [--keep]',
      );
      process.exit(0);
    }

    throw new Error(`Unknown argument: ${arg}`);
  }

  return args;
}

function run(command, args, options = {}) {
  return new Promise((resolve, reject) => {
    const child = spawn(command, args, {
      cwd: options.cwd ?? repoRoot,
      env: {
        ...process.env,
        ASPNETCORE_ENVIRONMENT: "Development",
      },
      shell: process.platform === "win32",
      stdio: "inherit",
    });

    child.on("error", reject);
    child.on("exit", (code) => {
      if (code === 0) {
        resolve();
      } else {
        reject(
          new Error(
            `${command} ${args.join(" ")} failed with exit code ${code}`,
          ),
        );
      }
    });
  });
}

async function walk(root) {
  const { readdir } = await import("node:fs/promises");
  const entries = await readdir(root, { withFileTypes: true });
  const results = [];

  for (const entry of entries) {
    const fullPath = path.join(root, entry.name);
    const relative = path.relative(root, fullPath).split(path.sep);
    if (relative.some((segment) => ignoredSegments.has(segment))) {
      continue;
    }

    results.push(fullPath);
    if (entry.isDirectory()) {
      results.push(...(await walk(fullPath)));
    }
  }

  return results;
}

async function scanPlaceholders(generatedRoot) {
  const matches = [];
  const entries = await walk(generatedRoot);

  for (const entry of entries) {
    const relative = path
      .relative(generatedRoot, entry)
      .split(path.sep)
      .join("/");
    if (
      knownPlaceholders.some((placeholder) => relative.includes(placeholder))
    ) {
      matches.push(`${relative} (path)`);
    }

    const entryStat = await stat(entry);
    if (!entryStat.isFile()) {
      continue;
    }

    const buffer = await readFile(entry);
    if (buffer.includes(0)) {
      continue;
    }

    const text = buffer.toString("utf8");
    for (const placeholder of knownPlaceholders) {
      if (text.includes(placeholder)) {
        matches.push(`${relative} (${placeholder})`);
      }
    }
  }

  return matches;
}

async function runFullValidation(generatedRoot) {
  const commands = [
    ["pnpm", ["install", "--frozen-lockfile"]],
    ["pnpm", ["format:check"]],
    ["openspec", ["validate", "--all", "--strict"]],
    ["dotnet", ["test", "AcmeDesk.slnx"]],
    ["pnpm", ["frontend:typecheck"]],
    ["pnpm", ["frontend:test"]],
    ["pnpm", ["frontend:build"]],
    ["pnpm", ["frontend:lint"]],
    ["pnpm", ["api-client:check"]],
  ];

  for (const [command, args] of commands) {
    await run(command, args, { cwd: generatedRoot });
  }
}

async function main() {
  const args = parseArgs(process.argv.slice(2));
  const tempRoot = await mkdtemp(
    path.join(os.tmpdir(), "modular-template-bootstrap-"),
  );
  const outputRoot = path.join(tempRoot, "acme-desk");

  try {
    await run("node", [
      path.join(repoRoot, "scripts", "bootstrap-template.mjs"),
      "--product-name",
      args.productName,
      "--output",
      outputRoot,
    ]);

    const matches = await scanPlaceholders(outputRoot);
    if (matches.length > 0) {
      throw new Error(
        `Known template placeholders remain:\n${matches.join("\n")}`,
      );
    }

    if (args.full) {
      await runFullValidation(outputRoot);
    } else {
      console.log(
        "Skipped full generated-repository validation. Re-run with --full to execute:",
      );
      console.log("- pnpm install --frozen-lockfile");
      console.log("- pnpm format:check");
      console.log("- openspec validate --all --strict");
      console.log("- dotnet test AcmeDesk.slnx");
      console.log("- pnpm frontend:typecheck");
      console.log("- pnpm frontend:test");
      console.log("- pnpm frontend:build");
      console.log("- pnpm frontend:lint");
      console.log("- pnpm api-client:check");
    }

    console.log(`Bootstrap verification passed: ${outputRoot}`);
  } finally {
    if (!args.keep) {
      await rm(tempRoot, { force: true, recursive: true });
    }
  }
}

main().catch((error) => {
  console.error(error.message);
  process.exit(1);
});
