'use client';
import { deleteAuction } from '@/app/actions/auctionActions';
import { Button } from 'flowbite-react';
import { useRouter } from 'next/navigation';
import React, { useState } from 'react';
import toast from 'react-hot-toast';

type Props = {
  id: string;
};
const DeleteButton = ({ id }: Props) => {
  const router = useRouter();

  async function onDeleteClick() {
    if (confirm('Are you sure you want to delete this Auction?')) {
      setLoading(true);
      try {
        const res = await deleteAuction(id);

        if (res.error) {
          throw res.error;
        }
        router.push(`/`);
      } catch (err: any) {
        toast.error(err.status + ' ' + err.message);
      } finally {
        setLoading(false);
      }
    }
  }

  const [loading, setLoading] = useState(false);
  return (
    <Button color='failure' onClick={onDeleteClick} isProcessing={loading}>
      Delete Auction
    </Button>
  );
};

export default DeleteButton;
