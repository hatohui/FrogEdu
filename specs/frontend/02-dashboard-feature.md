# Teacher Dashboard Feature

**Feature:** Teacher Dashboard Home & Overview  
**Services:** Multiple (User, Content, Assessment)  
**Version:** 1.0  
**Status:** Specification Phase

---

## Table of Contents

1. [Overview](#overview)
2. [User Stories](#user-stories)
3. [UI/UX Requirements](#uiux-requirements)
4. [Component Architecture](#component-architecture)
5. [Layout System](#layout-system)
6. [Implementation Tasks](#implementation-tasks)
7. [Acceptance Criteria](#acceptance-criteria)

---

## Overview

The Teacher Dashboard is the main hub for teachers after logging in. It provides an overview of recent activities, quick actions, and navigation to all major features.

### Responsibilities

- ✅ Display dashboard layout with sidebar navigation
- ✅ Show overview widgets (classes, exams, recent activity)
- ✅ Provide quick access to main features
- ✅ Display user profile in header
- ✅ Handle navigation between features
- ✅ Responsive design for mobile/tablet/desktop

---

## User Stories

### Story 2.1: Dashboard Overview

**As a** Teacher  
**I want to** see an overview of my classes and recent activities  
**So that** I can quickly understand my current status and access urgent tasks

**Acceptance Criteria:**

- [ ] Display number of active classes
- [ ] Show recent exams created
- [ ] List upcoming deadlines (if any)
- [ ] Quick action buttons for common tasks
- [ ] Recent activity feed
- [ ] Welcome message with user's name

### Story 2.2: Navigation

**As a** Teacher  
**I want to** easily navigate between different features  
**So that** I can efficiently use the platform

**Acceptance Criteria:**

- [ ] Sidebar shows all main features
- [ ] Active page is highlighted in sidebar
- [ ] Mobile: Hamburger menu for sidebar
- [ ] Logo click returns to dashboard
- [ ] User menu in header (profile, logout)

---

## UI/UX Requirements

### Layout: Dashboard Layout

**Desktop View:**

```
┌───────────────────────────────────────────────────────┐
│  [Logo]   Edu-AI Classroom         [User] [Logout]   │
├──────────┬────────────────────────────────────────────┤
│          │                                            │
│ Dashboard│  Dashboard                                 │
│ Content  │  Welcome back, Teacher Name!               │
│ Exams    │                                            │
│ Profile  │  ┌────────┐  ┌────────┐  ┌────────┐      │
│          │  │ Classes│  │ Exams  │  │Activity│      │
│          │  │   5    │  │   12   │  │   23   │      │
│          │  └────────┘  └────────┘  └────────┘      │
│          │                                            │
│          │  Quick Actions:                            │
│          │  [Create New Exam] [Browse Content]        │
│          │                                            │
│          │  Recent Activity:                          │
│          │  • Created exam "Math Quiz 1" - 2h ago    │
│          │  • Edited "Science Test" - 1 day ago      │
│          │                                            │
└──────────┴────────────────────────────────────────────┘
```

**Mobile View:**

```
┌─────────────────────────────────┐
│ [☰] Edu-AI     [User] [Logout] │
├─────────────────────────────────┤
│ Dashboard                       │
│ Welcome back, Teacher!          │
│                                 │
│ ┌────────────┐                 │
│ │  Classes   │                 │
│ │     5      │                 │
│ └────────────┘                 │
│ ┌────────────┐                 │
│ │   Exams    │                 │
│ │     12     │                 │
│ └────────────┘                 │
│                                 │
│ [Create New Exam]              │
│ [Browse Content]               │
│                                 │
└─────────────────────────────────┘
```

### Component: Sidebar Navigation

**Items:**

- Dashboard (Home icon)
- Content Library (Book icon)
- Exam Generator (FileText icon)
- Profile (User icon)

**Behavior:**

- Active item highlighted with background color
- Hover effect on items
- Smooth transitions
- Collapsible on mobile

### Component: Stat Card

```
┌────────────────┐
│  [Icon]        │
│                │
│  12            │
│  Active Exams  │
└────────────────┘
```

---

## Component Architecture

### Layout Tree

```
pages/
├── layout.tsx                      # Root layout (wraps entire app)
└── (dashboard)/
    ├── layout.tsx                  # Dashboard layout (sidebar + header)
    ├── dashboard/
    │   └── page.tsx               # Dashboard home page
    ├── content/
    │   └── ...                    # Content feature pages
    ├── assessment/
    │   └── ...                    # Assessment feature pages
    └── profile/
        └── page.tsx               # Profile page

features/dashboard/
├── components/
│   ├── DashboardStats.tsx         # Overview stats cards
│   ├── QuickActions.tsx           # Quick action buttons
│   ├── RecentActivity.tsx         # Activity feed
│   ├── StatCard.tsx               # Single stat card
│   └── WelcomeHeader.tsx          # Welcome message
├── hooks/
│   └── useDashboardStats.ts       # Query dashboard data
├── services/
│   └── dashboard.service.ts       # API calls for dashboard data
└── types/
    └── dashboard.types.ts         # TypeScript interfaces

components/layout/
├── RootLayout.tsx                 # Root layout (auth context, theme)
├── DashboardLayout.tsx            # Dashboard layout (sidebar, header)
├── Sidebar.tsx                    # Sidebar navigation
├── Header.tsx                     # Header with user menu
├── UserMenu.tsx                   # Dropdown menu (profile, logout)
└── MobileSidebar.tsx              # Mobile hamburger sidebar
```

### Component Implementations

#### 1. Dashboard Layout

```typescript
// pages/(dashboard)/layout.tsx

import { Outlet } from "react-router-dom";
import { Sidebar } from "@/components/layout/Sidebar";
import { Header } from "@/components/layout/Header";
import { MobileSidebar } from "@/components/layout/MobileSidebar";
import { useState } from "react";

export default function DashboardLayout() {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  return (
    <div className="min-h-screen bg-background">
      {/* Header */}
      <Header onMenuClick={() => setIsSidebarOpen(true)} />

      <div className="flex">
        {/* Desktop Sidebar */}
        <aside className="hidden md:block w-64 border-r border-border min-h-[calc(100vh-64px)]">
          <Sidebar />
        </aside>

        {/* Mobile Sidebar */}
        <MobileSidebar
          isOpen={isSidebarOpen}
          onClose={() => setIsSidebarOpen(false)}
        />

        {/* Main Content */}
        <main className="flex-1 p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
```

#### 2. Sidebar Component

```typescript
// components/layout/Sidebar.tsx

import { Link, useLocation } from "react-router-dom";
import { cn } from "@/utils/cn";
import { Home, BookOpen, FileText, User } from "lucide-react";

interface NavItem {
  label: string;
  href: string;
  icon: React.ComponentType<{ className?: string }>;
}

const navItems: NavItem[] = [
  { label: "Dashboard", href: "/dashboard", icon: Home },
  { label: "Content Library", href: "/content", icon: BookOpen },
  { label: "Exam Generator", href: "/assessment", icon: FileText },
  { label: "Profile", href: "/profile", icon: User },
];

export function Sidebar() {
  const location = useLocation();

  return (
    <nav className="flex flex-col gap-2 p-4">
      {navItems.map((item) => {
        const Icon = item.icon;
        const isActive =
          location.pathname === item.href ||
          location.pathname.startsWith(item.href + "/");

        return (
          <Link
            key={item.href}
            to={item.href}
            className={cn(
              "flex items-center gap-3 px-4 py-3 rounded-lg transition-colors",
              "hover:bg-accent hover:text-accent-foreground",
              isActive && "bg-accent text-accent-foreground font-medium"
            )}
          >
            <Icon className="h-5 w-5" />
            <span>{item.label}</span>
          </Link>
        );
      })}
    </nav>
  );
}
```

#### 3. Header Component

```typescript
// components/layout/Header.tsx

import { Menu } from "lucide-react";
import { Button } from "@/components/ui/button";
import { UserMenu } from "./UserMenu";

interface HeaderProps {
  onMenuClick: () => void;
}

export function Header({ onMenuClick }: HeaderProps) {
  return (
    <header className="h-16 border-b border-border flex items-center justify-between px-6">
      <div className="flex items-center gap-4">
        {/* Mobile Menu Button */}
        <Button
          variant="ghost"
          size="icon"
          className="md:hidden"
          onClick={onMenuClick}
        >
          <Menu className="h-6 w-6" />
        </Button>

        {/* Logo */}
        <div className="flex items-center gap-2">
          <img src="/logo.svg" alt="FrogEdu" className="h-8" />
          <span className="text-xl font-bold hidden sm:inline">
            Edu-AI Classroom
          </span>
        </div>
      </div>

      {/* User Menu */}
      <UserMenu />
    </header>
  );
}
```

#### 4. User Menu Component

```typescript
// components/layout/UserMenu.tsx

import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { useAuth } from "@/features/auth/hooks/useAuth";
import { useNavigate } from "react-router-dom";
import { User, LogOut } from "lucide-react";

export function UserMenu() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  if (!user) return null;

  const initials = user.fullName
    .split(" ")
    .map((n) => n[0])
    .join("")
    .toUpperCase();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <button className="flex items-center gap-2 hover:opacity-80 transition-opacity">
          <Avatar>
            <AvatarImage src={user.avatarUrl} />
            <AvatarFallback>{initials}</AvatarFallback>
          </Avatar>
          <span className="hidden sm:inline font-medium">{user.fullName}</span>
        </button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end" className="w-56">
        <DropdownMenuLabel>
          <div>
            <p className="font-medium">{user.fullName}</p>
            <p className="text-sm text-muted-foreground">{user.email}</p>
          </div>
        </DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuItem onClick={() => navigate("/profile")}>
          <User className="mr-2 h-4 w-4" />
          Profile
        </DropdownMenuItem>
        <DropdownMenuItem onClick={handleLogout}>
          <LogOut className="mr-2 h-4 w-4" />
          Logout
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
```

#### 5. Dashboard Page

```typescript
// pages/(dashboard)/dashboard/page.tsx

import { DashboardStats } from "@/features/dashboard/components/DashboardStats";
import { QuickActions } from "@/features/dashboard/components/QuickActions";
import { RecentActivity } from "@/features/dashboard/components/RecentActivity";
import { WelcomeHeader } from "@/features/dashboard/components/WelcomeHeader";
import { useDashboardStats } from "@/features/dashboard/hooks/useDashboardStats";
import { LoadingSpinner } from "@/components/common/LoadingSpinner";

export default function DashboardPage() {
  const { data: stats, isLoading } = useDashboardStats();

  if (isLoading) {
    return <LoadingSpinner />;
  }

  return (
    <div className="space-y-6">
      <WelcomeHeader />

      <DashboardStats stats={stats} />

      <QuickActions />

      <RecentActivity activities={stats?.recentActivities ?? []} />
    </div>
  );
}
```

#### 6. Dashboard Stats Component

```typescript
// features/dashboard/components/DashboardStats.tsx

import { StatCard } from "./StatCard";
import { BookOpen, FileText, Activity } from "lucide-react";
import type { DashboardStats as Stats } from "../types/dashboard.types";

interface DashboardStatsProps {
  stats?: Stats;
}

export function DashboardStats({ stats }: DashboardStatsProps) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
      <StatCard
        icon={BookOpen}
        value={stats?.classCount ?? 0}
        label="Active Classes"
        trend={stats?.classCountTrend}
      />
      <StatCard
        icon={FileText}
        value={stats?.examCount ?? 0}
        label="Exams Created"
        trend={stats?.examCountTrend}
      />
      <StatCard
        icon={Activity}
        value={stats?.activityCount ?? 0}
        label="Recent Activities"
      />
    </div>
  );
}
```

#### 7. Stat Card Component

```typescript
// features/dashboard/components/StatCard.tsx

import { Card, CardContent } from "@/components/ui/card";
import { LucideIcon } from "lucide-react";
import { cn } from "@/utils/cn";

interface StatCardProps {
  icon: LucideIcon;
  value: number;
  label: string;
  trend?: {
    value: number;
    isPositive: boolean;
  };
}

export function StatCard({ icon: Icon, value, label, trend }: StatCardProps) {
  return (
    <Card>
      <CardContent className="p-6">
        <div className="flex items-center justify-between mb-4">
          <div className="p-2 bg-primary/10 rounded-lg">
            <Icon className="h-6 w-6 text-primary" />
          </div>
          {trend && (
            <span
              className={cn(
                "text-sm font-medium",
                trend.isPositive ? "text-green-600" : "text-red-600"
              )}
            >
              {trend.isPositive ? "+" : ""}
              {trend.value}%
            </span>
          )}
        </div>
        <div>
          <p className="text-3xl font-bold">{value}</p>
          <p className="text-sm text-muted-foreground mt-1">{label}</p>
        </div>
      </CardContent>
    </Card>
  );
}
```

---

## Layout System

### Root Layout (All Pages)

```typescript
// pages/layout.tsx

import { Outlet } from "react-router-dom";
import { AuthProvider } from "@/features/auth/hooks/useAuth";
import { ThemeProvider } from "@/config/theme";
import { QueryClientProvider } from "@tanstack/react-query";
import { queryClient } from "@/config/tanstack";
import { Toaster } from "@/components/ui/sonner";

export default function RootLayout() {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider>
        <AuthProvider>
          <Outlet />
          <Toaster />
        </AuthProvider>
      </ThemeProvider>
    </QueryClientProvider>
  );
}
```

### Auth Layout (Login, Register)

```typescript
// pages/(auth)/layout.tsx

import { Outlet } from "react-router-dom";

export default function AuthLayout() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-primary/10 to-background">
      <Outlet />
    </div>
  );
}
```

---

## Implementation Tasks

### Milestone 1: Layout Infrastructure (Complete ✅)

#### Task 1.1: Install Shadcn Components - [x]

- [x] Install Avatar component
- [x] Install DropdownMenu component
- [x] Install Sheet component (mobile sidebar)
- [x] Install Separator component

#### Task 1.2: Create Root Layout - [x]

- [x] Wrap app with QueryClientProvider
- [x] Wrap app with ThemeProvider
- [x] Wrap app with AuthProvider
- [x] Add Toaster for notifications

**Validation:**

- [x] All providers work correctly
- [x] Context accessible throughout app

#### Task 1.3: Create Dashboard Layout - [x]

- [x] Create layout component with sidebar
- [x] Add responsive breakpoints
- [x] Handle mobile/desktop views

**Validation:**

- [x] Layout renders correctly on all screen sizes
- [x] Sidebar shows on desktop, hidden on mobile

#### Task 1.4: Create Sidebar Component - [x]

- [x] Add navigation items
- [x] Highlight active route
- [x] Add hover effects
- [x] Style with Tailwind

**Validation:**

- [x] Active route highlighted correctly
- [x] Navigation works
- [x] Hover effects smooth

#### Task 1.5: Create Mobile Sidebar - [x]

- [x] Use Sheet component (drawer)
- [x] Slide in from left
- [x] Close on navigation
- [x] Overlay background

**Validation:**

- [x] Opens/closes smoothly
- [x] Closes on outside click
- [x] Closes on navigation

#### Task 1.6: Create Header Component - [x]

- [x] Add logo
- [x] Add mobile menu button
- [x] Add user menu

**Validation:**

- [x] Logo click returns to dashboard
- [x] Mobile menu button works
- [x] User menu works

#### Task 1.7: Create User Menu - [x]

- [x] Display user avatar and name
- [x] Add dropdown with options
- [x] Implement logout

**Validation:**

- [x] Avatar displays correctly
- [x] Dropdown opens/closes
- [x] Logout works and redirects

### Milestone 2: Dashboard Page

#### Task 2.1: Create Dashboard Types - [ ]

- [ ] Define TypeScript interfaces
- [ ] Export from types file

#### Task 2.2: Create Dashboard Service - [ ]

- [ ] Implement API call for dashboard stats
- [ ] Handle errors

#### Task 2.3: Create useDashboardStats Hook - [ ]

- [ ] Query dashboard data
- [ ] Configure caching
- [ ] Handle loading/error

#### Task 2.4: Create WelcomeHeader Component - [ ]

- [ ] Display user's name
- [ ] Add greeting based on time of day
- [ ] Style with Tailwind

**Validation:**

- [ ] Name displays correctly
- [ ] Greeting appropriate

#### Task 2.5: Create StatCard Component - [ ]

- [ ] Display icon, value, label
- [ ] Add optional trend indicator
- [ ] Responsive design

**Validation:**

- [ ] Card displays all data
- [ ] Icons render correctly
- [ ] Responsive on all sizes

#### Task 2.6: Create DashboardStats Component - [ ]

- [ ] Grid of stat cards
- [ ] Fetch and display data
- [ ] Handle loading state

**Validation:**

- [ ] All stats display correctly
- [ ] Loading state shows

#### Task 2.7: Create QuickActions Component - [ ]

- [ ] Button to create new exam
- [ ] Button to browse content
- [ ] Navigate to correct pages

**Validation:**

- [ ] Buttons navigate correctly
- [ ] Styled consistently

#### Task 2.8: Create RecentActivity Component - [ ]

- [ ] List recent actions
- [ ] Format timestamps (relative)
- [ ] Handle empty state

**Validation:**

- [ ] Activities display correctly
- [ ] Timestamps formatted well
- [ ] Empty state shows

#### Task 2.9: Assemble Dashboard Page - [ ]

- [ ] Create page component
- [ ] Integrate all components
- [ ] Handle loading/error states

**Validation:**

- [ ] Page renders correctly
- [ ] All data displays
- [ ] Loading and error handling work

### Milestone 3: Polish & Accessibility

#### Task 3.1: Add Loading Skeletons - [ ]

- [ ] Create skeletons for stat cards
- [ ] Create skeleton for activity list
- [ ] Replace loading spinner

#### Task 3.2: Improve Mobile UX - [ ]

- [ ] Test all interactions on mobile
- [ ] Adjust spacing for touch targets
- [ ] Test hamburger menu

#### Task 3.3: Accessibility Audit - [ ]

- [ ] Add ARIA labels to navigation
- [ ] Test keyboard navigation
- [ ] Test with screen reader
- [ ] Fix any issues found

#### Task 3.4: Add Animations - [ ]

- [ ] Smooth sidebar transitions
- [ ] Fade in stat cards
- [ ] Animate mobile menu

---

## Acceptance Criteria

### Definition of Done

#### Functionality

- [ ] Dashboard layout renders correctly
- [ ] Navigation works on all pages
- [ ] Mobile sidebar works
- [ ] User menu works (profile, logout)
- [ ] Dashboard stats display correctly
- [ ] Quick actions navigate correctly

#### Code Quality

- [ ] All components typed correctly
- [ ] Layout system properly structured
- [ ] Responsive breakpoints work
- [ ] No prop drilling (use context when needed)

#### UX

- [ ] Smooth transitions
- [ ] Loading states on all data fetches
- [ ] Mobile UX is polished
- [ ] Hover effects feel responsive
- [ ] Active states are clear

#### Performance

- [ ] Dashboard loads in < 1 second
- [ ] No layout shift
- [ ] Smooth 60fps animations

#### Accessibility

- [ ] Keyboard navigation works
- [ ] ARIA labels present
- [ ] Focus indicators visible
- [ ] Screen reader compatible

---

**Next Feature:** [Smart Exam Generator](./04-assessment-feature.md)
