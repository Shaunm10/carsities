'use client';
import { Dropdown } from 'flowbite-react';
import { User } from 'next-auth';
import Link from 'next/link';
import React from 'react';
import { HiCog, HiUser } from 'react-icons/hi2';
import { AiFillCar, AiFillTrophy, AiOutlineLogout } from 'react-icons/ai';
import { signOut } from 'next-auth/react';
import { useRouter } from 'next/navigation';
import { usePathname } from 'next/navigation';
import { useParamsStore } from '@/hooks/useParamsStore';

type Props = {
  user: User;
};

export const UserActions = ({ user }: Props) => {
  const router = useRouter();
  const pathName = usePathname();
  const setParams = useParamsStore((state) => state.setParams);

  function setSeller() {
    // save this as the seller
    setParams({ seller: user.username, winner: undefined });

    // if the user isn't at the home page
    if (pathName !== '/') {
      // redirect them
      router.push('/');
    }
  }

  function setWinner() {
    // save this as the winner
    setParams({ winner: user.username, seller: undefined });

    // if the user isn't at the home page
    if (pathName !== '/') {
      // redirect them
      router.push('/');
    }
  }

  return (
    <Dropdown label={`Welcome ${user.name}`} inline>
      <Dropdown.Item icon={HiUser} onClick={setSeller}>
        My Auctions
      </Dropdown.Item>
      <Dropdown.Item icon={AiFillTrophy} onClick={setWinner}>
        Auctions won
      </Dropdown.Item>
      <Dropdown.Item icon={AiFillCar}>
        <Link href='/auctions/create'>Sell my car</Link>
      </Dropdown.Item>
      <Dropdown.Item icon={HiCog}>
        <Link href='/session'>Session (dev only)</Link>
      </Dropdown.Item>
      <Dropdown.Divider />
      <Dropdown.Item
        icon={AiOutlineLogout}
        onClick={() => signOut({ callbackUrl: '/' })}
      >
        <Link href='/session'>Sign out</Link>
      </Dropdown.Item>
    </Dropdown>
  );
};
