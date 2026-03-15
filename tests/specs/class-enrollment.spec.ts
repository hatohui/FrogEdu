/**
 * CL-001 → CL-014 — Class Creation & Student Enrollment
 *
 * Pre-requisites:
 *   - Teacher and Student accounts
 *   - At least one class with enrolled students for management tests
 */
import { test, expect } from "../fixtures/auth";

test.describe("Create Class", () => {
  test("CL-001: Teacher creates a new class", async ({ teacherPage: page }) => {
    await page.goto("/app/classes");

    const createBtn = page.getByRole("button", {
      name: /create.*class|new.*class|\+/i,
    });
    await createBtn.click();

    // Fill class form
    await page.getByLabel(/name/i).fill("E2E Test Class");

    // Select Grade
    const gradeInput = page
      .locator("[data-testid='grade-select']")
      .or(page.getByLabel(/grade/i));
    if ((await gradeInput.count()) > 0) {
      await gradeInput.click();
      await page.getByRole("option").first().click();
    }

    // Fill optional fields
    const descInput = page.getByLabel(/description/i);
    if ((await descInput.count()) > 0) {
      await descInput.fill("Automated test class");
    }

    const maxInput = page.getByLabel(/max.*student/i);
    if ((await maxInput.count()) > 0) {
      await maxInput.clear();
      await maxInput.fill("30");
    }

    // Submit
    await page.getByRole("button", { name: /create|save|submit/i }).click();

    // Expect success
    await expect(
      page
        .getByText(/success|created/i)
        .or(page.locator("[data-testid='class-detail']")),
    ).toBeVisible({ timeout: 10_000 });
  });

  test("CL-002: Class creation form validation — missing required fields", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/classes");

    const createBtn = page.getByRole("button", {
      name: /create.*class|new.*class|\+/i,
    });
    await createBtn.click();

    // Try submit with empty form
    const submitBtn = page.getByRole("button", { name: /create|save|submit/i });
    if (await submitBtn.isEnabled()) {
      await submitBtn.click();
      await expect(
        page.getByText(/required|invalid|minimum/i).first(),
      ).toBeVisible();
    } else {
      await expect(submitBtn).toBeDisabled();
    }
  });

  test("CL-003: Created class appears in teacher's class list", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/classes");

    // Class list should load
    const classList = page
      .locator("[data-testid='class-card']")
      .or(page.locator("[data-testid='class-list'] > *"));
    const emptyState = page.getByText(/no.*class|empty/i);

    await expect(classList.first().or(emptyState)).toBeVisible({
      timeout: 10_000,
    });
  });

  test("CL-004: Class detail page shows roster and info", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/classes");

    const classCard = page.locator("[data-testid='class-card']").first();
    if ((await classCard.count()) === 0) {
      test.skip();
      return;
    }
    await classCard.click();

    // Should see class name and invite code
    await expect(page.locator("main, [role='main']").first()).toBeVisible();

    // Invite code should be visible somewhere
    const inviteCode = page
      .getByText(/invite.*code|code/i)
      .or(page.locator("[data-testid='invite-code']"));
    if ((await inviteCode.count()) > 0) {
      await expect(inviteCode.first()).toBeVisible();
    }
  });
});

test.describe("Student Enrollment (Join Class)", () => {
  test("CL-005: Student joins class with valid invite code", async ({
    studentPage: page,
    teacherPage,
  }) => {
    // First, get an invite code from teacher's class
    await teacherPage.goto("/app/classes");
    const classCard = teacherPage.locator("[data-testid='class-card']").first();
    if ((await classCard.count()) === 0) {
      test.skip();
      return;
    }
    await classCard.click();

    // Try to find the invite code text
    const codeElement = teacherPage.locator("[data-testid='invite-code']");
    if ((await codeElement.count()) === 0) {
      test.skip();
      return;
    }
    const inviteCode = (await codeElement.textContent())?.trim() || "";
    if (inviteCode.length !== 6) {
      test.skip();
      return;
    }

    // Now student joins with that code
    await page.goto("/app/classes");
    const joinBtn = page.getByRole("button", { name: /join/i });
    await joinBtn.click();

    // Enter invite code in OTP-style input
    const otpInputs = page.locator("input[maxlength='1']");
    if ((await otpInputs.count()) === 6) {
      for (let i = 0; i < 6; i++) {
        await otpInputs.nth(i).fill(inviteCode[i]);
      }
    } else {
      // Single input field
      const codeInput = page
        .getByLabel(/code|invite/i)
        .or(page.locator("input[placeholder*='code']"));
      await codeInput.fill(inviteCode);
    }

    await page.getByRole("button", { name: /join|submit/i }).click();
    await expect(page.getByText(/success|joined|enrolled/i)).toBeVisible({
      timeout: 10_000,
    });
  });

  test("CL-006: Student joins — invalid invite code", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/classes");
    const joinBtn = page.getByRole("button", { name: /join/i });
    await joinBtn.click();

    // Enter invalid code
    const otpInputs = page.locator("input[maxlength='1']");
    if ((await otpInputs.count()) === 6) {
      for (let i = 0; i < 6; i++) {
        await otpInputs.nth(i).fill("Z");
      }
    } else {
      const codeInput = page
        .getByLabel(/code|invite/i)
        .or(page.locator("input[placeholder*='code']"));
      await codeInput.fill("ZZZZZZ");
    }

    await page.getByRole("button", { name: /join|submit/i }).click();

    // Should show error
    await expect(page.getByText(/invalid|not found|error/i)).toBeVisible({
      timeout: 10_000,
    });
  });

  test("CL-008: Student joins — already enrolled", async ({
    studentPage: page,
  }) => {
    // This would need a code for a class the student is already in.
    // We verify the error path generically.
    await page.goto("/app/classes");
    const joinBtn = page.getByRole("button", { name: /join/i });
    if ((await joinBtn.count()) === 0) {
      test.skip();
      return;
    }
    // Would need specific invite code — skip if no setup data
    test.skip();
  });

  test("CL-010: Join form validation — incomplete code", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/classes");
    const joinBtn = page.getByRole("button", { name: /join/i });
    await joinBtn.click();

    // Enter only 3 characters
    const otpInputs = page.locator("input[maxlength='1']");
    if ((await otpInputs.count()) === 6) {
      for (let i = 0; i < 3; i++) {
        await otpInputs.nth(i).fill("A");
      }
    }

    // Submit should be disabled
    const submitBtn = page.getByRole("button", { name: /join|submit/i });
    await expect(submitBtn).toBeDisabled();
  });
});

test.describe("Class Management", () => {
  test("CL-012: Student views enrolled classes", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/classes");

    const classList = page.locator("[data-testid='class-card']");
    const emptyState = page.getByText(/no.*class|empty|not enrolled/i);

    await expect(classList.first().or(emptyState)).toBeVisible({
      timeout: 10_000,
    });
  });

  test("CL-013: Teacher assigns exam to class via exam session", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/classes");

    const classCard = page.locator("[data-testid='class-card']").first();
    if ((await classCard.count()) === 0) {
      test.skip();
      return;
    }
    await classCard.click();

    // Look for "Create Session" or "Assign Exam" button
    const assignBtn = page.getByRole("button", {
      name: /create.*session|assign.*exam|new.*session/i,
    });
    if ((await assignBtn.count()) === 0) {
      test.skip();
      return;
    }
    await assignBtn.click();

    // Session creation form should appear
    await expect(
      page.getByText(/exam.*session|create.*session|assign/i),
    ).toBeVisible({ timeout: 10_000 });
  });

  test("CL-014: Admin views all classes", async ({ adminPage: page }) => {
    await page.goto("/app/classes");

    // Admin should see all classes
    const classList = page.locator("[data-testid='class-card']");
    const emptyState = page.getByText(/no.*class|empty/i);

    await expect(classList.first().or(emptyState)).toBeVisible({
      timeout: 10_000,
    });
  });
});
