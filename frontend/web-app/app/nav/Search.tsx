'use client';
import { useParamsStore } from '@/hooks/useParamsStore';
import React, { ChangeEvent, KeyboardEvent, useState } from 'react';
import { FaSearch } from 'react-icons/fa';
import { setEnvironmentData } from 'worker_threads';

export const Search = () => {
  const setParams = useParamsStore((state) => state.setParams);
  const [searchInput, setSearchInput] = useState('');

  function searchOnChange(event: ChangeEvent<HTMLInputElement>) {
    setSearchInput(event.target.value);
  }

  function search() {
    setParams({ searchTerm: searchInput });
  }
  return (
    <div className="flex w-[50%] items-center border-2 rounded-full py-2 shadow-sm">
      <input
        onKeyDown={(e: KeyboardEvent) => {
          if (e.key === 'Enter') {
            search();
          }
        }}
        onChange={searchOnChange}
        type="text"
        placeholder="Search for cars by make, model or color"
        className="
        flex-grow
        pl-5
        bg-transparent
        focus:outline-none
        border-transparent
        focus:border-transparent
        focus:ring-0
        text-sm
        text-gray-600
        "
      />
      <button onClick={search}>
        <FaSearch
          size={34}
          className="bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2"
        />
      </button>
    </div>
  );
};
