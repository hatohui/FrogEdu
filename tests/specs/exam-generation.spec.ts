/**
 * EG-001 → EG-016 — Exam Generation Flow
 *
 * Pre-requisites:
 *   - Teacher account with active subscription
 *   - At least one grade and subject configured
 *   - AI service running for question generation tests
 */
import { test, expect } from "../fixtures/auth";

test.describe("Create Exam", () => {
  test("EG-001: Teacher creates a new exam with basic info", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/exams");

    // Click create exam button
    const createBtn = page.getByRole("button", { name: /create|new|\+/i });
    await createBtn.click();

    // Fill exam form
    await page.getByLabel(/name/i).fill("E2E Test Exam");
    await page
      .getByLabel(/description/i)
      .fill(
        "This is an automated E2E test exam for verifying exam creation flow.",
      );

    // Select Grade
    const gradeSelect = page
      .locator("[data-testid='grade-select']")
      .or(page.getByLabel(/grade/i));
    if ((await gradeSelect.count()) > 0) {
      await gradeSelect.click();
      await page.getByRole("option").first().click();
    }

    // Select Subject (after grade loads)
    const subjectSelect = page
      .locator("[data-testid='subject-select']")
      .or(page.getByLabel(/subject/i));
    if ((await subjectSelect.count()) > 0) {
      await subjectSelect.click();
      await page.getByRole("option").first().click();
    }

    // Submit
    await page.getByRole("button", { name: /create|save|submit/i }).click();

    // Expect success (toast or redirect)
    await expect(
      page
        .getByText(/success|created/i)
        .or(page.locator("[data-testid='exam-detail']")),
    ).toBeVisible({ timeout: 10_000 });
  });

  test("EG-002: Cascade loading — Grade → Subject", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/exams");
    const createBtn = page.getByRole("button", { name: /create|new|\+/i });
    await createBtn.click();

    // Subject should be disabled or show placeholder before grade selected
    const subjectSelect = page
      .locator("[data-testid='subject-select']")
      .or(page.getByLabel(/subject/i));
    if ((await subjectSelect.count()) > 0) {
      // Select grade first
      const gradeSelect = page
        .locator("[data-testid='grade-select']")
        .or(page.getByLabel(/grade/i));
      await gradeSelect.click();
      await page.getByRole("option").first().click();

      // Now subject should be enabled and populated
      await expect(subjectSelect).toBeEnabled({ timeout: 5_000 });
    }
  });

  test("EG-003: Create exam form validation — missing required fields", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/exams");
    const createBtn = page.getByRole("button", { name: /create|new|\+/i });
    await createBtn.click();

    // Try to submit empty form
    const submitBtn = page.getByRole("button", { name: /create|save|submit/i });

    // Submit button should be disabled, or clicking shows validation errors
    if (await submitBtn.isEnabled()) {
      await submitBtn.click();
      // Expect validation messages
      await expect(
        page.getByText(/required|minimum|invalid/i).first(),
      ).toBeVisible();
    } else {
      await expect(submitBtn).toBeDisabled();
    }
  });

  test("EG-004: Update existing exam details", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/exams");

    // Click on a draft exam
    const examCard = page
      .locator("[data-testid='exam-card']")
      .first()
      .or(page.getByText(/draft/i).first());
    if ((await examCard.count()) === 0) {
      test.skip();
      return;
    }
    await examCard.click();

    // Click Edit
    const editBtn = page.getByRole("button", { name: /edit/i });
    if ((await editBtn.count()) === 0) {
      test.skip();
      return;
    }
    await editBtn.click();

    // Modify name
    const nameInput = page.getByLabel(/name/i);
    await nameInput.clear();
    await nameInput.fill("E2E Updated Exam Name");

    // Save
    await page.getByRole("button", { name: /save|update/i }).click();
    await expect(page.getByText(/success|updated/i)).toBeVisible({
      timeout: 10_000,
    });
  });

  test("EG-005: Delete a draft exam", async ({ teacherPage: page }) => {
    await page.goto("/app/exams");

    const examCard = page.locator("[data-testid='exam-card']").first();
    if ((await examCard.count()) === 0) {
      test.skip();
      return;
    }
    await examCard.click();

    const deleteBtn = page.getByRole("button", { name: /delete/i });
    if ((await deleteBtn.count()) === 0) {
      test.skip();
      return;
    }
    await deleteBtn.click();

    // Confirm dialog
    const confirmBtn = page
      .getByRole("button", { name: /confirm|yes|delete/i })
      .last();
    await confirmBtn.click();

    await expect(page.getByText(/deleted|removed|success/i)).toBeVisible({
      timeout: 10_000,
    });
  });
});

test.describe("Matrix & Question Generation", () => {
  test("EG-006: Teacher creates matrix for exam", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/exams");

    // Open an exam, then navigate to matrix builder
    const examCard = page.locator("[data-testid='exam-card']").first();
    if ((await examCard.count()) === 0) {
      test.skip();
      return;
    }
    await examCard.click();

    const matrixBtn = page.getByRole("button", {
      name: /matrix|specification/i,
    });
    if ((await matrixBtn.count()) === 0) {
      test.skip();
      return;
    }
    await matrixBtn.click();

    // Matrix builder should be visible
    await expect(
      page
        .locator("[data-testid='matrix-builder']")
        .or(page.getByText(/matrix|specification/i)),
    ).toBeVisible();
  });

  test("EG-008: AI generates questions from matrix", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/exams");
    const examCard = page.locator("[data-testid='exam-card']").first();
    if ((await examCard.count()) === 0) {
      test.skip();
      return;
    }
    await examCard.click();

    const generateBtn = page.getByRole("button", { name: /generate/i });
    if ((await generateBtn.count()) === 0) {
      test.skip();
      return;
    }
    await generateBtn.click();

    // Wait for AI generation (can be slow)
    await expect(page.getByText(/generated|success|complete/i)).toBeVisible({
      timeout: 60_000,
    });
  });
});

test.describe("Exam Questions & Publishing", () => {
  test("EG-013: Teacher manually adds questions to exam", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/exams");
    const examCard = page.locator("[data-testid='exam-card']").first();
    if ((await examCard.count()) === 0) {
      test.skip();
      return;
    }
    await examCard.click();

    // Look for add questions button
    const addBtn = page.getByRole("button", {
      name: /add.*question|question.*bank/i,
    });
    if ((await addBtn.count()) === 0) {
      test.skip();
      return;
    }
    await addBtn.click();

    // Select a question from the bank
    const checkbox = page
      .locator("[data-testid='question-checkbox']")
      .first()
      .or(page.locator("input[type='checkbox']").first());
    if ((await checkbox.count()) > 0) {
      await checkbox.check();
      await page.getByRole("button", { name: /add|confirm/i }).click();
    }
  });

  test("EG-015: Publish exam (draft → published)", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/exams");
    const examCard = page.locator("[data-testid='exam-card']").first();
    if ((await examCard.count()) === 0) {
      test.skip();
      return;
    }
    await examCard.click();

    const publishBtn = page.getByRole("button", { name: /publish/i });
    if ((await publishBtn.count()) === 0) {
      test.skip();
      return;
    }
    await publishBtn.click();

    // Confirm publish
    const confirmBtn = page
      .getByRole("button", { name: /confirm|yes|publish/i })
      .last();
    if ((await confirmBtn.count()) > 0) {
      await confirmBtn.click();
    }

    await expect(page.getByText(/published|success/i)).toBeVisible({
      timeout: 10_000,
    });
  });

  test("EG-016: Publish exam with 0 questions blocked", async ({
    teacherPage: page,
  }) => {
    // Create a new exam with no questions
    await page.goto("/app/exams");
    const createBtn = page.getByRole("button", { name: /create|new|\+/i });
    await createBtn.click();

    await page.getByLabel(/name/i).fill("Empty Exam for Publish Test");
    await page
      .getByLabel(/description/i)
      .fill("This exam has no questions and should not be publishable.");

    const gradeSelect = page
      .locator("[data-testid='grade-select']")
      .or(page.getByLabel(/grade/i));
    if ((await gradeSelect.count()) > 0) {
      await gradeSelect.click();
      await page.getByRole("option").first().click();
    }

    const subjectSelect = page
      .locator("[data-testid='subject-select']")
      .or(page.getByLabel(/subject/i));
    if ((await subjectSelect.count()) > 0) {
      await subjectSelect.click();
      await page.getByRole("option").first().click();
    }

    await page.getByRole("button", { name: /create|save|submit/i }).click();
    await page.waitForTimeout(2000);

    // Try to publish
    const publishBtn = page.getByRole("button", { name: /publish/i });
    if ((await publishBtn.count()) > 0) {
      await publishBtn.click();
      // Expect error
      await expect(
        page.getByText(/cannot publish|no questions|at least/i),
      ).toBeVisible({ timeout: 10_000 });
    }
  });
});
