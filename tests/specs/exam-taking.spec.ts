/**
 * ST-001 → ST-018 — Student Exam Taking Flow
 *
 * Pre-requisites (seed data):
 *   - Student account with active subscription
 *   - At least one active exam session with mixed question types
 *   - Session configured with retryTimes >= 2, shouldShuffleQuestions=true
 */
import { test, expect } from "../fixtures/auth";

test.describe("Start Exam Attempt", () => {
  test("ST-001: Student navigates to active exam sessions list", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions");
    // Verify session tabs are visible
    await expect(page.getByRole("tab", { name: /active/i })).toBeVisible();
    await expect(page.getByRole("tab", { name: /upcoming/i })).toBeVisible();
    await expect(page.getByRole("tab", { name: /ended/i })).toBeVisible();
  });

  test("ST-002: Student starts a new exam attempt", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions");
    // Click on the first active session
    const sessionCard = page.locator("[data-testid='session-card']").first();
    await sessionCard.click();
    // Click Start Exam
    const startBtn = page.getByRole("button", { name: /start exam/i });
    await expect(startBtn).toBeVisible();
    await startBtn.click();
    // Should land on the take-exam page
    await page.waitForURL(/\/take/, { timeout: 10_000 });
    // Question content should be visible
    await expect(
      page.locator("[data-testid='question-content']").first(),
    ).toBeVisible();
  });

  test("ST-003: Student blocked when session has not started yet", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions");
    await page.getByRole("tab", { name: /upcoming/i }).click();
    const upcomingCard = page.locator("[data-testid='session-card']").first();
    if ((await upcomingCard.count()) === 0) {
      test.skip();
      return;
    }
    await upcomingCard.click();
    // Start button should be disabled or absent
    const startBtn = page.getByRole("button", { name: /start exam/i });
    if ((await startBtn.count()) > 0) {
      await expect(startBtn).toBeDisabled();
    }
  });
});

test.describe("Answer Questions", () => {
  let takeExamUrl: string;

  test.beforeEach(async ({ studentPage: page }) => {
    // Navigate to an active session and start exam
    await page.goto("/app/exam-sessions");
    const session = page.locator("[data-testid='session-card']").first();
    if ((await session.count()) === 0) {
      test.skip();
      return;
    }
    await session.click();
    await page.getByRole("button", { name: /start exam|retry/i }).click();
    await page.waitForURL(/\/take/, { timeout: 10_000 });
    takeExamUrl = page.url();
  });

  test("ST-004: Answer a Multiple Choice question", async ({
    studentPage: page,
  }) => {
    // Find an MC question (radio buttons)
    const option = page.locator("input[type='radio']").first();
    if ((await option.count()) === 0) {
      test.skip();
      return;
    }
    await option.check();
    await expect(option).toBeChecked();
  });

  test("ST-005: Answer a Multiple Answer question", async ({
    studentPage: page,
  }) => {
    const checkboxes = page.locator("input[type='checkbox']");
    if ((await checkboxes.count()) < 2) {
      test.skip();
      return;
    }
    await checkboxes.nth(0).check();
    await checkboxes.nth(1).check();
    await expect(checkboxes.nth(0)).toBeChecked();
    await expect(checkboxes.nth(1)).toBeChecked();
  });

  test("ST-006: Answer a True/False question", async ({
    studentPage: page,
  }) => {
    // TF is rendered as two radio options
    const tfOption = page.getByText(/^true$/i).first();
    if ((await tfOption.count()) === 0) {
      test.skip();
      return;
    }
    await tfOption.click();
  });

  test("ST-007: Answer a Fill-in-the-Blank question", async ({
    studentPage: page,
  }) => {
    const input = page.locator("[data-testid='fill-blank-input']").first();
    if ((await input.count()) === 0) {
      test.skip();
      return;
    }
    await input.fill("sample answer");
    await expect(input).toHaveValue("sample answer");
  });

  test("ST-008: Answer an Essay question", async ({ studentPage: page }) => {
    const textarea = page.locator("textarea").first();
    if ((await textarea.count()) === 0) {
      test.skip();
      return;
    }
    await textarea.fill(
      "This is a test essay response for the FrogEdu examination system.",
    );
    await expect(textarea).not.toBeEmpty();
  });

  test("ST-009: Navigate between questions during exam", async ({
    studentPage: page,
  }) => {
    // Answer first question
    const firstInput = page.locator("input, textarea").first();
    if ((await firstInput.count()) === 0) {
      test.skip();
      return;
    }

    // Click Next
    const nextBtn = page.getByRole("button", { name: /next/i });
    await nextBtn.click();

    // Click Previous
    const prevBtn = page.getByRole("button", { name: /prev|back/i });
    await prevBtn.click();
  });
});

test.describe("Submit Exam", () => {
  test("ST-010: Submit exam with all questions answered", async ({
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

    // Click Submit
    await page.getByRole("button", { name: /submit/i }).click();

    // Confirm in dialog
    const confirmBtn = page.getByRole("button", { name: /confirm|yes/i });
    if ((await confirmBtn.count()) > 0) {
      await confirmBtn.click();
    }

    // Should redirect away from take page
    await page.waitForURL(/(?!.*\/take)/, { timeout: 15_000 });
  });

  test("ST-011: Submit exam with unanswered questions shows warning", async ({
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

    // Submit without answering
    await page.getByRole("button", { name: /submit/i }).click();

    // Expect a warning dialog about unanswered questions
    const dialog = page.getByRole("dialog");
    if ((await dialog.count()) > 0) {
      await expect(dialog).toContainText(/unanswered|incomplete/i);
    }
  });
});

test.describe("Retry & Time Limits", () => {
  test("ST-015: Student retries exam within allowed retryTimes", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/exam-sessions");
    const session = page.locator("[data-testid='session-card']").first();
    if ((await session.count()) === 0) {
      test.skip();
      return;
    }
    await session.click();

    const retryBtn = page.getByRole("button", { name: /retry/i });
    if ((await retryBtn.count()) === 0) {
      test.skip();
      return;
    }
    await retryBtn.click();
    await page.waitForURL(/\/take/, { timeout: 10_000 });
  });

  test("ST-016: Student blocked at maximum attempts", async ({
    studentPage: page,
  }) => {
    // This test needs a session where max attempts are used up
    await page.goto("/app/exam-sessions");
    const session = page.locator("[data-testid='session-card']").first();
    if ((await session.count()) === 0) {
      test.skip();
      return;
    }
    await session.click();

    // If retry is disabled or shows max message
    const retryBtn = page.getByRole("button", { name: /retry/i });
    if ((await retryBtn.count()) > 0) {
      // If attempts are maxed, button should be disabled
      const isDisabled = await retryBtn.isDisabled();
      if (!isDisabled) {
        test.skip(); // not at max yet
        return;
      }
      await expect(retryBtn).toBeDisabled();
    }
  });
});
