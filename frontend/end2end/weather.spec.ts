import { test, expect } from "@playwright/test";

test("should display weather information for Copenhagen", async ({ page }) => {
  // Start from home page
  await page.goto("/");
  
  // Click the Weather link
  await page.getByRole("link", { name: "Weather" }).click();
  
  // Verify we're on the weather page
  await expect(page).toHaveURL("/weather");
  
  // Verify the weather card title
  await expect(page.getByText("Weather in")).toBeVisible();
  
  // Verify temperature and wind speed labels are present
  await expect(page.getByText("Temperature:", { exact: true })).toBeVisible();
  await expect(page.getByText("Wind Speed:", { exact: true })).toBeVisible();
  
  // Verify the "Updated at" text is present
  await expect(page.getByText("Updated at:", { exact: false })).toBeVisible();
});