import assert from "node:assert/strict";
import { readdir, readFile } from "node:fs/promises";
import path from "node:path";
import { test } from "node:test";
import { fileURLToPath } from "node:url";

const scriptDir = path.dirname(fileURLToPath(import.meta.url));
const repoRoot = path.resolve(scriptDir, "..");
const templateDocsRoot = path.join(repoRoot, "template", "docs");
const currentStateRoot = path.join(templateDocsRoot, "current-state");

async function listMarkdownFiles(root) {
  const entries = await readdir(root, { withFileTypes: true });
  const files = [];

  for (const entry of entries) {
    const fullPath = path.join(root, entry.name);
    if (entry.isDirectory()) {
      files.push(...(await listMarkdownFiles(fullPath)));
      continue;
    }

    if (entry.isFile() && entry.name.endsWith(".md")) {
      files.push(fullPath);
    }
  }

  return files;
}

test("template documentation indexes current-state implementation progress", async () => {
  const index = await readFile(
    path.join(currentStateRoot, "README.md"),
    "utf8",
  );

  for (const fileName of [
    "server.md",
    "web.md",
    "platform.md",
    "identity.md",
  ]) {
    assert.match(index, new RegExp(`\\(${fileName}\\)`));
    await readFile(path.join(currentStateRoot, fileName), "utf8");
  }
});

test("stable template docs do not use current-state section headings", async () => {
  const docs = await listMarkdownFiles(templateDocsRoot);
  const stableDocs = docs.filter(
    (filePath) => !filePath.startsWith(currentStateRoot + path.sep),
  );
  const offenders = [];

  for (const filePath of stableDocs) {
    const content = await readFile(filePath, "utf8");
    const matches = content.match(/^##?\s+Current\b.*$/gim);
    if (matches) {
      offenders.push(
        `${path.relative(templateDocsRoot, filePath)}: ${matches.join(", ")}`,
      );
    }
  }

  assert.deepEqual(offenders, []);
});
