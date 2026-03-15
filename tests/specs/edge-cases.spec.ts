/**
 * EDGE-001 → EDGE-007 — Edge Cases & Negative Tests
 */
import { test, expect } from "../fixtures/auth";

test.describe("Submission Edge Cases", () => {
  test("EDGE-001: Network failure during exam submission", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions");
    const session = page.locator("[data-testid='session-card']").first();
    if ((await session.count()) === 0) {
      test.skip();
      return;
    }
    await session.click();
    await page.getByRole("button", { name: /start exam|retry/i }).click();
    await page.waitForURL(/\/take/, { timeout: 10_000 });

    // Simulate offline
    await page.context().setOffline(true);

    await page.getByRole("button", { name: /submit/i }).click();
    const confirmBtn = page.getByRole("button", { name: /confirm|yes/i });
    if ((await confirmBtn.count()) > 0) {
      await confirmBtn.click();
    }

    // Should show error or retry prompt (not crash)
    const errorMsg = page.getByText(/error|network|offline|retry/i);
    await expect(errorMsg.first()).toBeVisible({ timeout: 10_000 });

    // Restore network
    await page.context().setOffline(false);
  });
});

test.describe("Input Validation & Security", () => {
  test("EDGE-006: XSS attempt in FillInBlank answer", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions");
    const session = page.locator("[data-testid='session-card']").first();
    if ((await session.count()) === 0) {
      test.skip();
      return;
    }
    await session.click();
    await page.getByRole("button", { name: /start exam|retry/i }).click();
    await page.waitForURL(/\/take/, { timeout: 10_000 });

    // Find a fill-in-blank input
    const input = page
      .locator("[data-testid='fill-blank-input']")
      .first()
      .or(page.locator("input[type='text']").first());
    if ((await input.count()) === 0) {
      test.skip();
      return;
    }

    // Type XSS payload
    await input.fill('<script>alert("xss")</script>');

    // No alert dialog should appear
    let alertFired = false;
    page.on("dialog", () => {
      alertFired = true;
    });

    await page.waitForTimeout(1000);
    expect(alertFired).toBe(false);
  });

  test("EDGE-007: Expired session — attempt to start after endTime", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions");

    // Click on "Ended" tab
    await page.getByRole("tab", { name: /ended/i }).click();

    const endedSession = page.locator("[data-testid='session-card']").first();
    if ((await endedSession.count()) === 0) {
      test.skip();
      return;
    }
    await endedSession.click();

    // Start button should not be available or should be disabled
    const startBtn = page.getByRole("button", { name: /start exam/i });
    if ((await startBtn.count()) > 0) {
      await expect(startBtn).toBeDisabled();
    }
    // Or message about session ended
    const endedMsg = page.getByText(/ended|expired|closed/i);
    if ((await endedMsg.count()) > 0) {
      await expect(endedMsg.first()).toBeVisible();
    }
  });
});
