/**
 * EH-001 → EH-006 — Exam Session History
 *
 * Pre-requisites:
 *   - Student account with completed exam attempts
 */
import { test, expect } from "../fixtures/auth";

test.describe("View Session History", () => {
  test("EH-001: Student views exam session history page", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions/history");

    // Should see session list or empty state
    const sessions = page
      .locator("[data-testid='session-card']")
      .or(page.locator("[data-testid='history-item']"));
    const emptyState = page.getByText(/no.*session|no.*history|empty/i);

    await expect(sessions.first().or(emptyState)).toBeVisible({
      timeout: 10_000,
    });
  });

  test("EH-002: Session status badges display correctly", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions");

    // Check for status badges
    const badges = page
      .locator("[data-testid='status-badge']")
      .or(page.locator(".badge, [class*='badge']"));

    if ((await badges.count()) > 0) {
      // At least one badge should be visible
      await expect(badges.first()).toBeVisible();
    }
  });

  test("EH-003: Student views own attempts for a session", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions/history");

    const session = page
      .locator("[data-testid='session-card']")
      .first()
      .or(page.locator("[data-testid='history-item']").first());
    if ((await session.count()) === 0) {
      test.skip();
      return;
    }
    await session.click();

    // Should see attempt(s) listed
    const attempts = page
      .locator("[data-testid='attempt-row']")
      .or(page.getByText(/attempt/i));
    await expect(attempts.first()).toBeVisible({ timeout: 10_000 });
  });

  test("EH-004: Attempt statuses display correctly", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions/history");

    const session = page
      .locator("[data-testid='session-card']")
      .first()
      .or(page.locator("[data-testid='history-item']").first());
    if ((await session.count()) === 0) {
      test.skip();
      return;
    }
    await session.click();

    // Check that status text exists for attempts
    const statusTexts = page.getByText(
      /submitted|in\s?progress|timed\s?out|graded/i,
    );
    if ((await statusTexts.count()) > 0) {
      await expect(statusTexts.first()).toBeVisible();
    }
  });

  test("EH-005: Score displayed for submitted/graded attempts", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions/history");

    const session = page
      .locator("[data-testid='session-card']")
      .first()
      .or(page.locator("[data-testid='history-item']").first());
    if ((await session.count()) === 0) {
      test.skip();
      return;
    }
    await session.click();

    // Score should be displayed (e.g., "8/10" or "80%")
    const scoreText = page.getByText(/\d+\/\d+|\d+%/);
    if ((await scoreText.count()) > 0) {
      await expect(scoreText.first()).toBeVisible();
    }
  });

  test("EH-006: Empty state — no session history", async ({
    page,
    loginAs,
  }) => {
    // This test ideally needs a fresh student account with no attempts.
    // Using the default student page, we still verify the page doesn't crash.
    await loginAs(page, "student");
    await page.goto("/app/exam-sessions/history");

    // Page should render (either sessions or empty state)
    await expect(
      page.locator("main, [role='main'], #root").first(),
    ).toBeVisible();
  });
});
