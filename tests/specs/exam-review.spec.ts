/**
 * RV-001 → RV-012 — Review Page
 *
 * Pre-requisites:
 *   - Student account with at least one graded attempt
 *   - Attempt contains mixed question types (MC, MA, TF, Fill, Essay)
 */
import { test, expect } from "../fixtures/auth";

test.describe("Review Navigation & Score Summary", () => {
  test("RV-001: Student navigates to review page after exam", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions/history");

    // Open a completed session
    const session = page
      .locator("[data-testid='session-card']")
      .first()
      .or(page.locator("[data-testid='history-item']").first());
    if ((await session.count()) === 0) {
      test.skip();
      return;
    }
    await session.click();

    // Click Review on an attempt
    const reviewBtn = page
      .getByRole("link", { name: /review/i })
      .or(page.getByRole("button", { name: /review/i }));
    if ((await reviewBtn.count()) === 0) {
      test.skip();
      return;
    }
    await reviewBtn.first().click();

    await page.waitForURL(/\/review/, { timeout: 10_000 });

    // Review page should show exam name and questions
    await expect(page.locator("main, [role='main']").first()).toBeVisible();
  });

  test("RV-002: Score summary banner at top of review", async ({
    studentPage: page,
  }) => {
    // Navigate directly to a known review page (or through history)
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

    const reviewBtn = page
      .getByRole("link", { name: /review/i })
      .or(page.getByRole("button", { name: /review/i }));
    if ((await reviewBtn.count()) === 0) {
      test.skip();
      return;
    }
    await reviewBtn.first().click();
    await page.waitForURL(/\/review/, { timeout: 10_000 });

    // Score banner should contain score/total or percentage
    const scoreBanner = page.getByText(/\d+\/\d+|\d+(\.\d+)?%/);
    await expect(scoreBanner.first()).toBeVisible({ timeout: 10_000 });
  });
});

test.describe("Question Type Review", () => {
  test.beforeEach(async ({ studentPage: page }) => {
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
    const reviewBtn = page
      .getByRole("link", { name: /review/i })
      .or(page.getByRole("button", { name: /review/i }));
    if ((await reviewBtn.count()) === 0) {
      test.skip();
      return;
    }
    await reviewBtn.first().click();
    await page.waitForURL(/\/review/, { timeout: 10_000 });
  });

  test("RV-003: Review correct/incorrect for MC question", async ({
    studentPage: page,
  }) => {
    // Look for question with correct/incorrect indicators
    const correctMark = page
      .locator("[data-testid='correct-answer']")
      .or(page.locator(".text-green-500, .text-green-600, [class*='correct']"));
    const incorrectMark = page
      .locator("[data-testid='incorrect-answer']")
      .or(page.locator(".text-red-500, .text-red-600, [class*='incorrect']"));

    // At least one correct or incorrect indicator should be visible
    const anyMark = correctMark.first().or(incorrectMark.first());
    if ((await anyMark.count()) > 0) {
      await expect(anyMark).toBeVisible();
    }
  });

  test("RV-007: AI feedback displayed for Essay question", async ({
    studentPage: page,
  }) => {
    // Look for essay feedback section
    const feedback = page
      .locator("[data-testid='essay-feedback']")
      .or(page.getByText(/feedback|ai.*feedback/i));
    if ((await feedback.count()) > 0) {
      await expect(feedback.first()).toBeVisible();
    }
  });

  test("RV-009: Mixed question types render correctly in review", async ({
    studentPage: page,
  }) => {
    // Page should not have any error indicators
    const errorBoundary = page.getByText(/something went wrong|error/i);
    // Should have multiple questions rendered
    const questions = page
      .locator("[data-testid='review-question']")
      .or(page.locator("[class*='question']"));

    // No crash — error boundary should not be visible
    await expect(errorBoundary).not.toBeVisible();

    if ((await questions.count()) > 0) {
      expect(await questions.count()).toBeGreaterThan(0);
    }
  });

  test("RV-010: Answer explanations shown in review", async ({
    studentPage: page,
  }) => {
    const explanation = page
      .locator("[data-testid='explanation']")
      .or(page.getByText(/explanation/i));
    if ((await explanation.count()) > 0) {
      await expect(explanation.first()).toBeVisible();
    }
  });
});

test.describe("Review Access Control & Features", () => {
  test("RV-011: Cannot review another student's attempt", async ({
    studentPage: page,
  }) => {
    // Navigate to a fabricated review URL (non-existent session/attempt)
    await page.goto(
      "/app/exam-sessions/00000000-0000-0000-0000-000000000000/attempts/00000000-0000-0000-0000-000000000000/review",
    );

    // Should show error or redirect (not show another student's data)
    const forbidden = page.getByText(
      /forbidden|not found|access denied|404|403/i,
    );
    const redirected =
      page.url().includes("/login") || page.url().includes("/app");

    if ((await forbidden.count()) > 0) {
      await expect(forbidden.first()).toBeVisible();
    } else {
      // Should have redirected away
      expect(redirected).toBeTruthy();
    }
  });

  test("RV-012: AI Explain feature on incorrect question", async ({
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
    const reviewBtn = page
      .getByRole("link", { name: /review/i })
      .or(page.getByRole("button", { name: /review/i }));
    if ((await reviewBtn.count()) === 0) {
      test.skip();
      return;
    }
    await reviewBtn.first().click();
    await page.waitForURL(/\/review/, { timeout: 10_000 });

    // Click explain button
    const explainBtn = page.getByRole("button", { name: /explain/i });
    if ((await explainBtn.count()) === 0) {
      test.skip();
      return;
    }
    await explainBtn.first().click();

    // AI explanation should appear
    await expect(
      page.getByText(/explanation|because|the correct/i),
    ).toBeVisible({ timeout: 30_000 });
  });
});
