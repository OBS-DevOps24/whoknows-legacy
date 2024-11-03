import { test } from "@playwright/test";

test("test", async ({ page }) => {
  await page.goto("http://localhost:5173/");
  await page.getByRole("link", { name: "¿Who Knows?" }).click();
  await page.getByRole("link", { name: "Register" }).click();
  await page.getByPlaceholder("Username").click();
  await page.getByPlaceholder("Username").fill("testuser1234");
  await page.getByPlaceholder("E-Mail").click();
  await page.getByPlaceholder("E-Mail").fill("testuser1234@mail.dk");
  await page.getByPlaceholder("Password", { exact: true }).click();
  await page.getByPlaceholder("Password", { exact: true }).fill("test1234");
  await page.getByPlaceholder("Password (repeat)").click();
  await page.getByPlaceholder("Password (repeat)").fill("test1234");
  await page.getByRole("button", { name: "Sign Up" }).click();
});
