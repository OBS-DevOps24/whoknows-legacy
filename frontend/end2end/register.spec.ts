import { test, expect } from "@playwright/test";

test("should register a new user successfully", async ({ page }) => {
  const timestamp = Date.now();
  const username = `e2etest_${timestamp}`;
  const email = `e2etest_${timestamp}@example.com`;
  
  await page.goto("/");
  await page.getByRole("link", { name: "Register" }).click();
  await page.getByPlaceholder("Username").fill(username);
  await page.getByPlaceholder("E-Mail").fill(email);
  await page.getByPlaceholder("Password", { exact: true }).fill("test1234");
  await page.getByPlaceholder("Password (repeat)").fill("test1234");
  await page.getByRole("button", { name: "Sign Up" }).click();

  await expect(page).toHaveURL("/", { timeout: 10000 });
  // Check for logged-in state by looking for "Log out" in the nav
  await expect(page.locator("#nav-logout")).toBeVisible();
});
