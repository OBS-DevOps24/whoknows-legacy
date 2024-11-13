import { test, expect } from "@playwright/test";

test("should login successfully with test account", async ({ page }) => {
  const testUsername = "e2e_test_user";
  const testPassword = "e2e_test_password123";

  await page.goto("/");
  await page.getByRole("link", { name: "Log in" }).click();
  await page.getByPlaceholder("Username").fill(testUsername);
  await page.getByPlaceholder("Password").fill(testPassword);
  await page.getByRole("button", { name: "Log In" }).click();

  // After successful login, we should be redirected to home
  await expect(page).toHaveURL("/", { timeout: 10000 });
  
  // Check for logged-in state by looking for "Log out" in the nav
  await expect(page.locator("#nav-logout")).toBeVisible();
});

test("should show error message with wrong credentials", async ({ page }) => {
  await page.goto("/");
  await page.getByRole("link", { name: "Log in" }).click();
  await page.getByPlaceholder("Username").fill("wronguser");
  await page.getByPlaceholder("Password").fill("wrong");
  await page.getByRole("button", { name: "Log In" }).click();

  await expect(page.locator('[role="alert"]')).toBeVisible();
  await expect(page.locator('[role="alert"]')).toContainText("Invalid username or password");
});
