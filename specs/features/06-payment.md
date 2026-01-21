# Feature 06: Payment & Subscription

**Status:** 📝 Spec Phase  
**Priority:** P2 (High - Revenue)  
**Effort:** 16-20 hours

---

## 1. Overview

Monetization layer. Handles upgrades to "Pro Teacher" plan via VNPay (Vietnam) or Stripe. Manages limits (Free vs Pro).

**Services:** `Payment Service` (New), `User Service` (Role/Claims)  
**Frontend:** `/pricing`, `/billing`

---

## 2. User Stories

| ID  | As a... | I want to...        | So that...                          |
| --- | ------- | ------------------- | ----------------------------------- |
| 6.1 | Teacher | Upgrade to Pro      | I get unlimited exams and storage.  |
| 6.2 | System  | Enforce Free Limits | Users are incentivized to upgrade.  |
| 6.3 | Teacher | Pay via VNPay/QR    | It's convenient for local payments. |

---

## 3. Technical Specifications

### 3.1 Domain Model

**Aggregate:** `Subscription`

- **Properties:** `UserId`, `PlanType` (Free/Pro), `StartDate`, `EndDate`, `Provider` (VNPay/Stripe).
- **Limits:**
  - Free: 3 Exams/mo, 1 Class.
  - Pro: Unlimited.

### 3.2 Integration Flow (VNPay)

1.  Client clicks "Upgrade" -> API creates Transaction.
2.  API redirects to VNPay Gateway.
3.  User pays.
4.  VNPay calls `IPN URL` (Webhook).
5.  Backend verifies signature -> Updates User `PlanType`.

---

## 4. Implementation Checklist

### 4.1 Backend (Payment Service)

- [ ] **Domain**: `Subscription`, `Transaction` entities.
- [ ] **Infrastructure**:
  - [ ] `VNPayService`: Hash generator, URL builder.
  - [ ] `StripeService` (Optional V2).
- [ ] **API**:
  - [ ] `POST /api/payment/create-url`
  - [ ] `GET /api/payment/ipn` (Webhook)
  - [ ] `GET /api/payment/return` (UI Redirect)

### 4.2 Frontend (Payment Feature)

- [ ] **Pages**:
  - [ ] `PricingPage`: Plan comparison cards.
  - [ ] `PaymentSuccessPage`.
  - [ ] `BillingSettings`: Show current plan/expiry.
- [ ] **Guards**:
  - [ ] `LimitGuard`: Check usage (e.g., 3/3 exams) -> Show Upgrade Modal.

---

## 5. Acceptance Criteria

- [ ] Successful payment immediately unlocks Pro features.
- [ ] IPN Webhook handles secure verification (Checksum).
- [ ] Users exceeding limits see neat "Upgrade" prompt.
