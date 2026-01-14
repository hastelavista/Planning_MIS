// ============================================
// FILE: src/components/Sidebar.tsx
// ============================================
'use client';

import React from 'react';
import { Home, Settings, Database, FileText, BarChart } from 'lucide-react';
import { MenuItem } from './menuItem';
import type { MenuItem as MenuItemType } from '@/types';

interface SidebarProps {
  isOpen: boolean;
}

export const Sidebar: React.FC<SidebarProps> = ({ isOpen }) => {
  const menuItems: MenuItemType[] = [
    {
      label: 'Dashboard',
      icon: <Home className="w-5 h-5" />,
      route: '/dashboard',
    },
    {
      label: 'General Setup',
      icon: <Settings className="w-5 h-5" />,
      children: [
        { label: 'Configuration', route: '/dashboard/general/configuration', icon: null },
        { label: 'Users', route: '/dashboard/general/users', icon: null },
        { label: 'Roles', route: '/dashboard/general/roles', icon: null },
      ],
    },
    {
      label: 'Master Setup',
      icon: <Database className="w-5 h-5" />,
      children: [
        { label: 'Categories', route: '/dashboard/master/categories', icon: null },
        { label: 'Products', route: '/dashboard/master/products', icon: null },
        { label: 'Suppliers', route: '/dashboard/master/suppliers', icon: null },
      ],
    },
    {
      label: 'Reports',
      icon: <FileText className="w-5 h-5" />,
      children: [
        { label: 'Sales Report', route: '/dashboard/reports/sales', icon: null },
        { label: 'Inventory Report', route: '/dashboard/reports/inventory', icon: null },
      ],
    },
  ];

  return (
    <aside
      className={`${
        isOpen ? 'w-64' : 'w-20'
      } bg-white border-r border-gray-200 transition-all duration-300 ease-in-out h-full overflow-y-auto`}
    >
      <div className="p-4 space-y-2">
        {menuItems.map((item, idx) => (
          <MenuItem key={idx} item={item} isOpen={isOpen} />
        ))}
      </div>
    </aside>
  );
};