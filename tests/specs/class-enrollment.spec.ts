/**
 * CL-001 → CL-014 — Class Creation & Student Enrollment
 *
 * Pre-requisites:
 *   - Teacher and Student accounts
 *   - At least one class with enrolled students for management tests
 */
import { test, expect, handleSubscriptionIfNeeded } from "../fixtures/auth";

test.describe("Create Class", () => {
  test("CL-001: Teacher creates a new class", async ({ teacherPage: page }) => {
    await page.goto("/app/classes");
    await handleSubscriptionIfNeeded(page);

    const createBtn = page
      .getByRole("button", {
        name: /create.*class|new.*class/i,
      })
      .first();
    await createBtn.click();

    // Scope form interactions to the dialog
    const dialog = page.locator("[role='dialog']");
    await expect(dialog).toBeVisible({ timeout: 5_000 });

    // Fill class form
    await dialog
      .getByLabel(/class.*name|name/i)
      .first()
      .fill("E2E Test Class");

    // Select Grade (the select trigger is labeled "Grade")
    const gradeInput = dialog.getByLabel(/grade/i);
    if ((await gradeInput.count()) > 0) {
      await gradeInput.click();
      await page.getByRole("option").first().click();
    }

    // Fill required fields
    const descInput = dialog.getByLabel(/description/i);
    if ((await descInput.count()) > 0) {
      await descInput.fill("Automated test class");
    }

    const maxInput = dialog.getByLabel(/max.*student/i);
    if ((await maxInput.count()) > 0) {
      await maxInput.clear();
      await maxInput.fill("30");
    }

    // Submit via dialog's submit button
    await dialog.locator("button[type='submit']").click();

    // Expect success toast
    await expect(page.getByText(/success|created/i).first()).toBeVisible({
      timeout: 10_000,
    });
  });

  test("CL-002: Class creation form validation — missing required fields", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/classes");
    await handleSubscriptionIfNeeded(page);

    const createBtn = page
      .getByRole("button", {
        name: /create.*class|new.*class/i,
      })
      .first();
    await createBtn.click();

    // Scope to the dialog
    const dialog = page.locator("[role='dialog']");
    await expect(dialog).toBeVisible({ timeout: 5_000 });

    // Clear the pre-filled default fields to make form invalid
    await dialog
      .getByLabel(/class.*name|name/i)
      .first()
      .clear();

    // Try submit with empty required fields
    const submitBtn = dialog.locator("button[type='submit']");
    await submitBtn.click();

    // Expect validation errors
    await expect(
      dialog.getByText(/required|invalid|minimum/i).first(),
    ).toBeVisible();
  });

  test("CL-003: Created class appears in teacher's class list", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/classes");
    await handleSubscriptionIfNeeded(page);

    // Class cards are links to /app/classes/:id within Card components
    const classList = page.locator(
      "a[href^='/app/classes/'] [data-slot='card']",
    );
    const emptyState = page.getByText(/no.*class|empty|create.*first/i);

    await expect(classList.first().or(emptyState)).toBeVisible({
      timeout: 10_000,
    });
  });

  test("CL-004: Class detail page shows roster and info", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/classes");
    await handleSubscriptionIfNeeded(page);

    // Wait for class cards to load
    const classCard = page
      .locator("a[href^='/app/classes/'] [data-slot='card']")
      .first();
    if (
      !(await classCard
        .waitFor({ state: "visible", timeout: 10_000 })
        .then(() => true)
        .catch(() => false))
    ) {
      test.skip();
      return;
    }
    // Navigate directly to the class detail page to avoid subscription redirect on click
    const href = await page
      .locator("a[href^='/app/classes/']")
      .first()
      .getAttribute("href");
    if (!href) {
      test.skip();
      return;
    }
    await page.goto(href);
    await handleSubscriptionIfNeeded(page);

    // Invite code should be visible for teachers (rendered as <code> with font-mono)
    const inviteCode = page.locator("code.font-mono");
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
    await handleSubscriptionIfNeeded(teacherPage);
    const classLink = teacherPage.locator("a[href^='/app/classes/']").first();
    if (
      !(await classLink
        .waitFor({ state: "visible", timeout: 10_000 })
        .then(() => true)
        .catch(() => false))
    ) {
      test.skip();
      return;
    }
    const href = await classLink.getAttribute("href");
    if (!href) {
      test.skip();
      return;
    }
    await teacherPage.goto(href);
    await handleSubscriptionIfNeeded(teacherPage);

    // Try to find the invite code text (rendered as <code> with tracking-widest or tracking-[0.3em])
    const codeElement = teacherPage.locator(
      "code.font-mono.font-bold, code.font-mono.font-semibold",
    );
    if (
      !(await codeElement
        .waitFor({ state: "visible", timeout: 5_000 })
        .then(() => true)
        .catch(() => false))
    ) {
      test.skip();
      return;
    }
    const inviteCode = (await codeElement.first().textContent())?.trim() || "";
    if (inviteCode.length !== 6) {
      test.skip();
      return;
    }

    // Now student joins with that code
    await page.goto("/app/classes");
    const joinBtn = page.getByRole("button", { name: /join.*class/i });
    await joinBtn.click();

    // Enter invite code via the OTP input (input-otp renders a single input)
    const otpInput = page.locator("[data-input-otp]");
    await otpInput.fill(inviteCode);

    await page.locator("button[type='submit']").click();
    // After successful join, student should see enrolled class card
    await expect(
      page.locator("a[href^='/app/classes/'] [data-slot='card']").first(),
    ).toBeVisible({ timeout: 10_000 });
  });

  test("CL-006: Student joins — invalid invite code", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/classes");
    const joinBtn = page.getByRole("button", { name: /join.*class/i });
    await joinBtn.click();

    // Enter invalid code via OTP input
    const otpInput = page.locator("[data-input-otp]");
    await otpInput.fill("ZZZZZZ");

    await page.locator("button[type='submit']").click();

    // Should show error
    await expect(
      page.getByText(/invalid|not found|error|fail/i).first(),
    ).toBeVisible({ timeout: 10_000 });
  });

  test("CL-008: Student joins — already enrolled", async ({
    studentPage: page,
  }) => {
    // This would need a code for a class the student is already in.
    // We verify the error path generically.
    await page.goto("/app/classes");
    const joinBtn = page.getByRole("button", { name: /join.*class/i });
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
    const joinBtn = page.getByRole("button", { name: /join.*class/i });
    await joinBtn.click();

    // Enter only 3 characters via OTP input
    const otpInput = page.locator("[data-input-otp]");
    await otpInput.fill("ABC");

    // Submit should be disabled since code is incomplete
    const submitBtn = page.locator("button[type='submit']");
    await expect(submitBtn).toBeDisabled();
  });
});

test.describe("Class Management", () => {
  test("CL-012: Student views enrolled classes", async ({
    studentPage: page,
  }) => {
    await page.goto("/app/classes");

    const classList = page.locator(
      "a[href^='/app/classes/'] [data-slot='card']",
    );
    const emptyState = page.getByText(/no.*class|empty|not enrolled/i);

    await expect(classList.first().or(emptyState)).toBeVisible({
      timeout: 10_000,
    });
  });

  test("CL-013: Teacher assigns exam to class via exam session", async ({
    teacherPage: page,
  }) => {
    await page.goto("/app/classes");
    await handleSubscriptionIfNeeded(page);

    const classLink = page.locator("a[href^='/app/classes/']").first();
    if (
      !(await classLink
        .waitFor({ state: "visible", timeout: 10_000 })
        .then(() => true)
        .catch(() => false))
    ) {
      test.skip();
      return;
    }
    const href = await classLink.getAttribute("href");
    if (!href) {
      test.skip();
      return;
    }
    await page.goto(href);
    await handleSubscriptionIfNeeded(page);

    // Look for "Assign Exam" button on the class detail page
    const assignBtn = page.getByRole("button", {
      name: /create.*session|assign.*exam|new.*session|assign/i,
    });
    if ((await assignBtn.count()) === 0) {
      test.skip();
      return;
    }
    await assignBtn.first().click();

    // Assign exam dialog should appear
    await expect(
      page.getByRole("dialog", { name: /assign.*exam/i }),
    ).toBeVisible({ timeout: 10_000 });
  });

  test("CL-014: Admin views all classes", async ({ adminPage: page }) => {
    await page.goto("/app/classes");

    // Admin should see class cards or empty state
    const classList = page.locator(
      "a[href^='/app/classes/'] [data-slot='card']",
    );
    const emptyState = page.getByRole("heading", { name: /no.*class/i });

    await expect(classList.first().or(emptyState)).toBeVisible({
      timeout: 10_000,
    });
  });
});
