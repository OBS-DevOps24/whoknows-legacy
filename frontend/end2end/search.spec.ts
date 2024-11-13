import { test, expect } from "@playwright/test";

test("should show results when searching for 'javascript'", async ({ page }) => {
  await page.goto("/");
  
  // Get the search input and type "javascript"
  const searchInput = page.getByPlaceholder("Search...");
  await searchInput.fill("javascript");
  
  // Find and click the search button
  const searchButton = page.getByRole("button", { name: "Search" });
  await searchButton.click();
  
  // Check for the search query text
  await expect(page.getByText("Showing results for javascript", { exact: false }))
    .toBeVisible({ timeout: 10000 });
    
  // Check for search results
  const searchResults = page.locator('.hover\\:bg-zinc-100'); // Using the hover class from your component
  
  // Verify we have at least one result
  await expect(searchResults.first()).toBeVisible({ timeout: 10000 });
  
  // Verify result structure
  const firstResult = searchResults.first();
  await expect(firstResult.locator('h2 a')).toBeVisible(); // Title should be visible
  await expect(firstResult.locator('p')).toBeVisible();    // Content should be visible
  await expect(firstResult.locator('a.text-green-900')).toBeVisible(); // URL should be visible
});

test("should handle empty search", async ({ page }) => {
  await page.goto("/");
  
  // Click search with empty input
  const searchButton = page.getByRole("button", { name: "Search" });
  await searchButton.click();
  
  // Verify the "Showing results for" text is not present
  await expect(page.getByText("Showing results for", { exact: false }))
    .not.toBeVisible();
});

test("should handle search with no results", async ({ page }) => {
  await page.goto("/");
  
  // Search for something that shouldn't exist
  const searchInput = page.getByPlaceholder("Search...");
  await searchInput.fill("xyzabc123nonexistent");
  
  const searchButton = page.getByRole("button", { name: "Search" });
  await searchButton.click();
  
  // Verify "Showing results for" text appears
  await expect(page.getByText("Showing results for xyzabc123nonexistent", { exact: false }))
    .toBeVisible({ timeout: 10000 });
    
  // Verify no results are shown
  const searchResults = page.locator('.hover\\:bg-zinc-100');
  await expect(searchResults).toHaveCount(0, { timeout: 10000 });
});