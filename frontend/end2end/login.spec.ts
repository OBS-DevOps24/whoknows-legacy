import { test, expect } from "@playwright/test";

test("should login successfully with test account", async ({ page }) => {
  const testUsername = "e2e_test_user";
  const testPassword = "e2e_test_password123";

  await page.goto("/");
  await page.getByRole("link", { name: "Log in" }).click();
  await page.getByPlaceholder("Username").fill(testUsername);
  await page.getByPlaceholder("Password").fill(testPassword);
  
  // Click login button and wait for BOTH:
  // 1. The API response
  // 2. The navigation that happens after login
  await Promise.all([
    // Wait for the API response
    page.waitForResponse(
      response => response.url().includes('/api/login')
    ),
    // Trigger the login
    page.getByRole("button", { name: "Log In" }).click(),
  ]);

  // First verify we're logged in by checking for logout button
  await expect(page.locator("#nav-logout")).toBeVisible();

  // Now check which page we ended up on
  const currentUrl = page.url();
  if (currentUrl.includes('/change-password')) {
    // We were redirected to change-password - this means that the users password expired
    await expect(page).toHaveURL("/change-password");
  } else {
    // We should be on the home page - this means that the users password is still valid
    await expect(page).toHaveURL("/");
  }
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
