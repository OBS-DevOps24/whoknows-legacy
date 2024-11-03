import { test, expect } from "@playwright/test";

test("test", async ({ page }) => {
  await page.goto("http://localhost:5173/");
  await page.getByRole("link", { name: "Log in" }).click();
  await page.getByPlaceholder("Username").click();
  await page.getByPlaceholder("Username").fill("testuser1234");
  await page.getByPlaceholder("Password").click();
  await page.getByPlaceholder("Password").fill("test1234");
  await page.getByRole("button", { name: "Log In" }).click();

  // Optionally, you can also assert the current URL
  expect(page.url()).toBe("http://localhost:5173");
});
