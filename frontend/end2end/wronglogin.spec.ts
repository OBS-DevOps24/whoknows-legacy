import { test } from "@playwright/test";

test("test", async ({ page }) => {
  await page.goto("http://localhost:5173/");
  await page.getByRole("link", { name: "Log in" }).click();
  await page.getByPlaceholder("Username").click();
  await page.getByPlaceholder("Username").fill("wrongunser");
  await page.getByPlaceholder("Username").press("Tab");
  await page.getByPlaceholder("Password").fill("wrong");
  await page.getByRole("button", { name: "Log In" }).click();
});
