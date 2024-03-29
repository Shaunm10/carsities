'use client';
import { useParamsStore } from '@/hooks/useParamsStore';
import { usePathname, useRouter } from 'next/navigation';
import React, { ChangeEvent, KeyboardEvent, useState } from 'react';
import { FaSearch } from 'react-icons/fa';

export const Search = () => {
  const router = useRouter();
  const pathname = usePathname();
  const setParams = useParamsStore((state) => state.setParams);
  //const [searchInput, setSearchInput] = useState('');

  /*** What we use to *Write* the search value */
  const setSearchValue = useParamsStore((state) => state.setSearchValue);

  /** What we use to *Read* the search value */
  const searchValue = useParamsStore((state) => state.searchValue);

  //search;
  function searchOnChange(event: ChangeEvent<HTMLInputElement>) {
    setSearchValue(event.target.value);
    //setSearchInput(event.target.value);
  }

  function search() {
    // if they are not on the home page
    if (pathname !== '/') {
      // than send them back.
      router.push('/');
    }
    setParams({ searchTerm: searchValue });
  }

  return (
    <div className='flex w-[50%] items-center border-2 rounded-full py-2 shadow-sm'>
      <input
        onKeyDown={(e: KeyboardEvent) => {
          if (e.key === 'Enter') {
            search();
          }
        }}
        value={searchValue}
        onChange={searchOnChange}
        type='text'
        placeholder='Search for cars by make, model or color'
        className='
        flex-grow
        pl-5
        bg-transparent
        focus:outline-none
        border-transparent
        focus:border-transparent
        focus:ring-0
        text-sm
        text-gray-600
        '
      />
      <button onClick={search}>
        <FaSearch
          size={34}
          className='bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2'
        />
      </button>
    </div>
  );
};
