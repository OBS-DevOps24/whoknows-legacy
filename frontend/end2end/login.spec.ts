import { test, expect } from "@playwright/test";

test("should login successfully with test account", async ({ page }) => {
  const testUsername = "e2e_test_user";
  const testPassword = "e2e_test_password123";

  await page.goto("/");
  await page.getByRole("link", { name: "Log in" }).click();
  await page.getByPlaceholder("Username").fill(testUsername);
  await page.getByPlaceholder("Password").fill(testPassword);
  
  // Click login button and wait for everything
  await Promise.all([
    // Wait for the API response
    page.waitForResponse(
      response => response.url().includes('/api/login')
    ),
    // Wait for navigation away from login page
    page.waitForURL(url => !url.pathname.includes('/login'), { timeout: 30000 }),
    // Trigger the login
    page.getByRole("button", { name: "Log In" }).click(),
  ]);

  // Make sure we're logged in
  await expect(page.locator("#nav-logout")).toBeVisible({ timeout: 30000 });

  // Wait for any redirects to complete
  await page.waitForLoadState('networkidle');
  
  // Now check which page we ended up on
  const currentUrl = new URL(page.url());
  // Verify we're either on home page or change-password page
  expect(
    ["/", "/change-password"].includes(currentUrl.pathname),
    `Expected URL to be either / or /change-password but was ${currentUrl.pathname}`
  ).toBeTruthy();
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
