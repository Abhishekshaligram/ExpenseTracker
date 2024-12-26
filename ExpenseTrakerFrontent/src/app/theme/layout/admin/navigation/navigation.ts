import { Injectable } from '@angular/core';

export interface NavigationItem {
  id: string;
  title: string;
  type: 'item' | 'collapse' | 'group';
  icon?: string;
  url?: string;
  classes?: string;
  external?: boolean;
  target?: boolean;
  breadcrumbs?: boolean;
  children?: Navigation[];
}

export interface Navigation extends NavigationItem {
  children?: NavigationItem[];
}
const NavigationItems = [
  {
    id: 'dashboard',
    title: 'Dashboard',
    type: 'group',
    icon: 'icon-navigation',
    children: [
      {
        id: 'default',
        title: 'Dashboard',
        type: 'item',
        classes: 'nav-item',
        url: '/default',
        icon: 'ti ti-dashboard',
        breadcrumbs: false
      }
    ]
  },
  // {
  //   id: 'page',
  //   title: 'Expense Traker Module',
  //   type: 'group',
  //   icon: 'icon-navigation',
  //   children: [
  //     {
  //       id: 'Authentication',
  //       title: 'ETM',
  //       type: 'collapse',
  //       icon: 'ti ti-key',
  //       children: [
  //         {
  //           id: 'Expense',
  //           title: 'Expense',
  //           type: 'item',
  //           url: '/etm/expense-list',
  //           target: false,
  //           breadcrumbs: false
  //         },
  //         {
  //           id: 'user',
  //           title: 'User',
  //           type: 'item',
  //           url: '/etm/user-list',
  //           target: true,
  //           breadcrumbs: false
  //         },
  //         {
  //           id: 'category',
  //           title: 'Category',
  //           type: 'item',
  //           url: '/etm/category-list',
  //           target: true,
  //           breadcrumbs: false
  //         }
  //       ]
  //     }
  //   ]
  // },
  {
    id: 'elements',
    title: 'Expense Traker Module',
    type: 'group',
    icon: 'icon-navigation',
    children: [
      {
        id: 'user',
        title: 'User',
        type: 'item',
        classes: 'nav-item',
        url: '/user-list',
        icon: 'ti ti-user'
      },
      {
        id: 'category',
        title: 'Category',
        type: 'item',
        classes: 'nav-item',
        url: '/category-list',
        icon: 'ti ti-tag'
      },
      {
        id: 'expense',
        title: 'Expense',
        type: 'item',
        classes: 'nav-item',
        url: '/expense-list',
        icon: 'ti ti-wallet'
      },
      
    ]
  },

  {
    id: 'elements',
    title: 'Budget Management Module',
    type: 'group',
    icon: 'icon-navigation',
    children: [
      {
        id: 'expense',
        title: 'Budget Management',
        type: 'item',
        classes: 'nav-item',
        url: '/budget-list',
        icon: 'pi pi-credit-card'
      },
      
    ]
  },

  {
    id: 'elements',
    title: 'Report Module',
    type: 'group',
    icon: 'icon-navigation',
    children: [
      {
        id: 'expense',
        title: 'Monthly-Expense',
        type: 'item',
        classes: 'nav-item',
        url: '/report-expense-list',
        icon: 'pi pi-money-bill'
      },
      {
        id: 'expense',
        title: 'Monthly-Report',
        type: 'item',
        classes: 'nav-item',
        url: '/expense-report',
        icon: 'pi pi-calendar'
      },
      
    ]
  },
];

@Injectable()
export class NavigationItem {
  get() {
    return NavigationItems;
  }
}
