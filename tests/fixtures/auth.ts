/**
 * Shared auth fixtures for FrogEdu E2E tests.
 *
 * Test credentials are read from environment variables:
 *   STUDENT_EMAIL / STUDENT_PASSWORD
 *   TEACHER_EMAIL / TEACHER_PASSWORD
 *   ADMIN_EMAIL   / ADMIN_PASSWORD
 */
import { test as base, expect, type Page } from "@playwright/test";

export interface TestAccounts {
  student: { email: string; password: string };
  teacher: { email: string; password: string };
  admin: { email: string; password: string };
}

function accounts(): TestAccounts {
  return {
    student: {
      email: process.env.STUDENT_EMAIL || "admin",
      password: process.env.STUDENT_PASSWORD || "admin",
    },
    teacher: {
      email: process.env.TEACHER_EMAIL || "admin",
      password: process.env.TEACHER_PASSWORD || "admin",
    },
    admin: {
      email: process.env.ADMIN_EMAIL || "admin",
      password: process.env.ADMIN_PASSWORD || "admin",
    },
  };
}

/** Handle subscription selection page if visible after navigation settles. */
export async function handleSubscriptionIfNeeded(page: Page): Promise<void> {
  // Wait for client-side route guards / SPA redirects to settle
  await page.waitForLoadState("networkidle");
  if (page.url().includes("select-subscription")) {
    await page
      .getByRole("button", { name: /continue.*free/i })
      .click({ timeout: 5_000 });
    await page.waitForURL(/\/(app|dashboard)/, { timeout: 15_000 });
  }
}

/** Fill the /login form and wait for redirect. */
async function loginViaUI(
  page: Page,
  email: string,
  password: string,
): Promise<void> {
  await page.goto("/login");
  await page.getByRole("textbox", { name: /email/i }).fill(email);
  await page.locator('input[name="password"]').fill(password);
  await page.locator('button[type="submit"]').click();
  // Wait for post-login navigation to reach a known destination
  await page.waitForURL(/\/(app|dashboard|select-subscription)/, {
    timeout: 30_000,
  });
  // Handle subscription selection page if the account hasn't chosen a plan yet
  await handleSubscriptionIfNeeded(page);
}

// --------------- Fixtures ---------------

type AuthFixtures = {
  accounts: TestAccounts;
  studentPage: Page;
  teacherPage: Page;
  adminPage: Page;
  loginAs: (page: Page, role: "student" | "teacher" | "admin") => Promise<void>;
};

export const test = base.extend<AuthFixtures>({
  accounts: async ({}, use) => {
    await use(accounts());
  },

  loginAs: async ({}, use) => {
    await use(async (page, role) => {
      const creds = accounts()[role];
      await loginViaUI(page, creds.email, creds.password);
    });
  },

  studentPage: async ({ browser }, use) => {
    const ctx = await browser.newContext();
    const page = await ctx.newPage();
    const { email, password } = accounts().student;
    await loginViaUI(page, email, password);
    await use(page);
    await ctx.close();
  },

  teacherPage: async ({ browser }, use) => {
    const ctx = await browser.newContext();
    const page = await ctx.newPage();
    const { email, password } = accounts().teacher;
    await loginViaUI(page, email, password);
    await use(page);
    await ctx.close();
  },

  adminPage: async ({ browser }, use) => {
    const ctx = await browser.newContext();
    const page = await ctx.newPage();
    const { email, password } = accounts().admin;
    await loginViaUI(page, email, password);
    await use(page);
    await ctx.close();
  },
});

export { expect };
