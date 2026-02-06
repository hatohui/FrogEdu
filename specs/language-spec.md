# Frontend Internationalization (i18n) Implementation

## Objective

Implement comprehensive multi-language support for the FrogEdu frontend application, supporting English and Vietnamese, with user-controlled language switching accessible from both navigation bar and user settings.

## Current State

- Language configuration files exist: `frontend/public/languages/en.json` and `frontend/public/languages/vi.json`
- i18n infrastructure is partially established in `frontend/src/config/i18n.tsx`
- Language hook exists: `frontend/src/languages/index.ts`
- No documented hardcoded strings audit exists

## Scope

### Included

- Audit and identify all hardcoded strings in React components, pages, and services
- Extract hardcoded strings to translation files (en.json and vi.json)
- Ensure language switcher functionality in user settings page
- Ensure language switcher functionality in navigation bar
- Implement persistent language preference (localStorage)
- Test language switching across all user-facing text

### Excluded

- Error messages from external API responses (log only, no translation needed)
- Console logs for debugging
- Placeholder text in development-only components
- Third-party UI library default text (use library's i18n if available)

## Detailed Requirements

### 1. String Audit & Extraction

- [ ] Scan all `.tsx` and `.ts` files in `frontend/src/` for hardcoded strings (text visible to users)
- [ ] Document findings with file path and line number
- [ ] Categorize strings by feature/domain (common, navigation, pages, forms, etc.)
- [ ] Identify strings requiring context/notes for translators (e.g., button labels vs. headings)

### 2. Translation File Structure

Maintain consistent structure in `en.json` and `vi.json`:

```json
{
  "common": { "label": "value" },
  "navigation": { "label": "value" },
  "pages": { "pageName": { "label": "value" } },
  "forms": { "fieldName": { "label": "value", "placeholder": "value" } },
  "errors": { "errorCode": "message" },
  "buttons": { "actionName": "label" }
}
```

### 3. Component Integration

- [ ] Import translation hook from `frontend/src/languages/index.ts` in all components with hardcoded strings
- [ ] Replace hardcoded strings with `i18n` or translation function calls
- [ ] Example: `<button>{i18n?.common?.submit || 'Submit'}</button>`
- [ ] Add fallback values for missing translations

### 4. Language Switcher - Navigation Bar

- [ ] Add language selector dropdown/toggle in navigation component
- [ ] Display current language (EN/VI or flag icon)
- [ ] Update language in real-time without page reload
- [ ] Persist selection to localStorage with key: `selectedLanguage`
- [ ] Location: Navigation bar (typically header component)

### 5. Language Switcher - User Settings

- [ ] Add language preference option in user settings/profile page
- [ ] Allow users to select from: English, Vietnamese
- [ ] Sync with navigation bar switcher (should reflect current selection)
- [ ] Optionally persist to user profile/backend if user authentication implemented
- [ ] Location: Settings/Profile page under "Preferences" section

### 6. Implementation Details

#### i18n Function Pattern

```typescript
// Usage in components
const { i18n } = useTranslation(); // or existing hook from i18n.tsx
const text = i18n?.section?.key || "Fallback Text";
```

#### localStorage Integration

```typescript
const LANGUAGE_KEY = "selectedLanguage";
const DEFAULT_LANGUAGE = "en";

// Get stored language
const savedLanguage = localStorage.getItem(LANGUAGE_KEY) || DEFAULT_LANGUAGE;

// Set language
localStorage.setItem(LANGUAGE_KEY, languageCode);
```

#### Language Context/Hook

Ensure the existing i18n setup in `frontend/src/config/i18n.tsx` provides:

- Current language state
- Language change handler
- Automatic translation file loading
- Fallback to English if key missing in Vietnamese

## Acceptance Criteria

- [x] All user-visible text in components is extracted to translation files
- [x] No hardcoded strings remain in React JSX (except error boundaries/debug-only code)
- [x] Language switcher appears in navigation bar and is functional
- [x] Language switcher appears in user settings and is functional
- [x] Switching language updates all text immediately (no page reload required)
- [x] Selected language persists across page refreshes (localStorage)
- [x] All translations (EN/VI) exist for all keys in translation files
- [x] Fallback English text displays if translation key missing
- [x] Language preference syncs between navigation bar and settings page

## Testing Checklist

- [ ] Switch language from navigation bar → verify all text updates
- [ ] Switch language from user settings → verify all text updates
- [ ] Refresh page → verify saved language persists
- [ ] Check all pages (home, exams, classes, etc.) for untranslated text
- [ ] Test with browser DevTools language set to different locales
- [ ] Verify form labels, placeholders, button text all update
- [ ] Test with long text in Vietnamese (layout/overflow issues)

## Affected Files (Estimated)

- `frontend/public/languages/en.json` (expand existing)
- `frontend/public/languages/vi.json` (expand existing)
- `frontend/src/config/i18n.tsx` (verify/enhance)
- `frontend/src/languages/index.ts` (verify/enhance)
- ALL component files under `frontend/src/components/` and `frontend/src/pages/`
- Navigation component(s) in `frontend/src/components/layout/`
- User settings/profile page

## Dependencies

- i18n configuration must be functional (_already exists_)
- localStorage must be accessible (standard browser API)
- React context or state management for language state
