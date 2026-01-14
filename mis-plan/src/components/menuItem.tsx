'use client';

import React, { useState } from 'react';
import { ChevronDown, ChevronRight } from 'lucide-react';
import { useRouter } from 'next/navigation';
import type { MenuItem as MenuItemType } from '@/types';

interface MenuItemProps {
  item: MenuItemType;
  isOpen: boolean;
}

export const MenuItem: React.FC<MenuItemProps> = ({ item, isOpen }) => {
  const [expanded, setExpanded] = useState(false);
  const router = useRouter();
  const hasChildren = item.children && item.children.length > 0;

  const handleClick = () => {
    if (hasChildren) {
      setExpanded(!expanded);
    } else if (item.route) {
      router.push(item.route);
    }
  };

  return (
    <div>
      <button
        onClick={handleClick}
        className="w-full flex items-center justify-between px-4 py-3 text-gray-700 hover:bg-indigo-50 hover:text-indigo-600 transition-colors rounded-lg"
      >
        <div className="flex items-center gap-3">
          {item.icon}
          {isOpen && <span className="font-medium">{item.label}</span>}
        </div>
        {isOpen && hasChildren && (
          expanded ? <ChevronDown className="w-4 h-4" /> : <ChevronRight className="w-4 h-4" />
        )}
      </button>

      {isOpen && expanded && hasChildren && item.children &&(
        <div className="ml-4 mt-1 space-y-1">
          {item.children.map((child, idx) => (
            <button
              key={idx}
              onClick={() => child.route && router.push(child.route)}
              className="w-full flex items-center gap-3 px-4 py-2 text-sm text-gray-600 hover:bg-indigo-50 hover:text-indigo-600 transition-colors rounded-lg"
            >
              <div className="w-2 h-2 rounded-full bg-gray-400"></div>
              {child.label}
            </button>
          ))}
        </div>
      )}
    </div>
  );
};
