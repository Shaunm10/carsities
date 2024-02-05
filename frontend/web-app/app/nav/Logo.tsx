'use client';
import { useParamsStore } from '@/hooks/useParamsStore';
import { usePathname, useRouter } from 'next/navigation';
import React from 'react';
import { AiOutlineCar } from 'react-icons/ai';

export const Logo = () => {
  const resetSearchState = useParamsStore((state) => state.reset);

  const redirectToHomeAndResetState = () => {
    // if we aren't on the home page
    if (pathname !== '/') {
      // navigate us there.
      router.push('/');
    }
    // like always reset the state.
    resetSearchState();
  };

  const router = useRouter();
  const pathname = usePathname();
  return (
    <div
      onClick={redirectToHomeAndResetState}
      className='cursor-pointer flex items-center gap-2 text-3xl font-semibold text-red-500'
    >
      <AiOutlineCar size={34} />
      <div>Carsties Auctions</div>
    </div>
  );
};
