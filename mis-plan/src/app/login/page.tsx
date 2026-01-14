'use client';

import React, { useState, useEffect } from 'react';
import { useAuth } from '@/contexts/authContext';
import Image from 'next/image';
import { officeAPI } from '@/lib/api';

export default function LoginPage() {
  const { login } = useAuth();
  const [generalSetup, setGeneralSetup] = useState<any>(null);
  const [loadingSetup, setLoadingSetup] = useState(true);


  // const generalSetup = {
  //   MainHeading: "Main Heading Example",
  //   Name: "Office Name",
  //   SubHeading3: "Sub Heading",
  //   Address1: "Kathmandu",
  //   Address2: "Nepal",
  //   Footer: "© 2025 MIS Planning",
  //   Photo: "/common/images/gov_logo.png",
  //   Background: "/common/images/login_bg.jpg"
  // };

  const [credentials, setCredentials] = useState({ username: '', password: '' });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

   useEffect(() => {
    async function loadOffice() {
      try {
        const data = await officeAPI.getLoginOfficeDetails();
        setGeneralSetup(data);
      } catch (error) {
        console.error("Failed to load setup", error);
      } finally {
        setLoadingSetup(false);
      }
    }
    loadOffice();
  }, []);

  if (loadingSetup) {
    return <div className="p-10 text-center text-xl">Loading...</div>;
  }
  if (!generalSetup) {
    return <div className="p-10 text-center text-xl text-red-500">Failed to load office details</div>;
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      await login(credentials);
    } catch (err) {
      setError('Invalid username or password');
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-cover bg-center flex items-center justify-end px-25"
      style= {{ backgroundImage: "url('/common/images/login_bg.jpg')" }}
    >
      <div className="w-full max-w-md bg-white/90 backdrop-blur p-6 rounded-lg shadow-xl">
        
        {/* Logo */}
        <div className="text-center mb-4">
          <Image
            src={generalSetup.photo}
            alt="Office Logo"
            width={90}
            height={90}
            className="mx-auto rounded h-auto w-auto"
          />
        </div>

        {/* Headings */}
        <h1 className="text-2xl font-bold text-center text-gray-800">
          {generalSetup.mainHeading}
        </h1>

        <h2 className="text-lg text-center text-gray-600">
          {generalSetup.name}
        </h2>

        <h3 className="text-md text-center text-gray-600">
          {generalSetup.subHeading3}
        </h3>

        <p className="text-sm text-center text-gray-500">
          {(generalSetup.address1 || "") + " - " + (generalSetup.address2 || "")}
        </p>

        {/* Error */}
        {error && (
          <div className="bg-red-100 border border-red-300 text-red-700 p-2 rounded mt-3 text-sm">
            {error}
          </div>
        )}

        {/* Form */}
        <form className="mt-5 space-y-4" onSubmit={handleSubmit}>
          
          <div>
            <label className="block text-sm font-medium text-gray-700">
              प्रयोगकर्ता नाम
            </label>
            <input
              type="text"
              className="w-full px-4 py-2 mt-1 border border-gray-300 rounded-lg 
                         text-gray-900 placeholder-gray-400
                         focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
              placeholder="इमेल वा प्रयोगकर्ताको नाम"
              value={credentials.username}
              onChange={(e) => setCredentials({ ...credentials, username: e.target.value })}
              disabled={loading}
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">
              पासवर्ड
            </label>
            <input
              type="password"
              className="w-full px-4 py-2 mt-1 border border-gray-300 rounded-lg 
                         text-gray-900 placeholder-gray-400
                         focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
              placeholder="पासवर्ड"
              value={credentials.password}
              onChange={(e) => setCredentials({ ...credentials, password: e.target.value })}
              disabled={loading}
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-indigo-600 text-white py-2 rounded-lg 
                       font-medium hover:bg-indigo-700 transition
                       disabled:opacity-50"
          >
            {loading ? "Signing in..." : "साइन इन"}
          </button>
        </form>

        {/* Footer & Support */}
        <div className="mt-5 text-center text-sm text-gray-700">
          <p>{generalSetup.Footer || "© 2025 MIS Planning"}</p>
          <button
            onClick={() => alert("Support number here")}
            className="text-indigo-600 font-semibold mt-2 underline"
          >
            सपोट सम्पर्क नम्बर
          </button>
        </div>
      </div>
    </div>
  );
}
