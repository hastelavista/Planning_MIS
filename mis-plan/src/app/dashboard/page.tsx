// ============================================
// FILE: src/app/dashboard/page.tsx
// ============================================
'use client';

import React from 'react';

export default function DashboardPage() {
  return (
    <div className="p-6">
      <h2 className="text-2xl font-bold text-gray-800 mb-4">Dashboard</h2>
      
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-6">
        {[
          { label: 'Total Users', value: '1,234', color: 'text-blue-600' },
          { label: 'Active Projects', value: '56', color: 'text-green-600' },
          { label: 'Pending Tasks', value: '89', color: 'text-yellow-600' },
          { label: 'Completed', value: '234', color: 'text-indigo-600' },
        ].map((stat, idx) => (
          <div key={idx} className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
            <p className="text-gray-600 text-sm mb-2">{stat.label}</p>
            <p className={`text-3xl font-bold ${stat.color}`}>{stat.value}</p>
          </div>
        ))}
      </div>

      <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
        <h3 className="text-lg font-semibold text-gray-800 mb-4">Recent Activity</h3>
        <p className="text-gray-600">Welcome to MIS Planning Dashboard!</p>
      </div>
    </div>
  );
}