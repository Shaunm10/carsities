'use client';
import { Button, TextInput } from 'flowbite-react';
import React, { useEffect } from 'react';
import { FieldValues, useForm } from 'react-hook-form';
import Input from '../components/input';
import DateInput from '../components/DateInput';

export const AuctionForm = () => {
  const {
    /**a wrapper around 'onsubmit' that this hook can use to verify how to submit a form */
    handleSubmit,
    control,
    /**Allows us to default the focus on a control */
    setFocus,
    formState: { isDirty, isSubmitting, isValid, errors },
  } = useForm({ mode: 'onTouched' });

  function onSubmit(data: FieldValues) {
    console.log(data);
  }

  useEffect(() => {
    setFocus('make');
  }, [setFocus]);

  return (
    <form className='flex flex-col mt-3' onSubmit={handleSubmit(onSubmit)}>
      <Input
        label='Make'
        name='make'
        control={control}
        rules={{ required: 'Make is required' }}
      />
      <Input
        label='Model'
        name='model'
        control={control}
        rules={{ required: 'Model is required' }}
      />
      <Input
        label='Color'
        name='color'
        control={control}
        rules={{ required: 'Color is required' }}
      />
      <div className='grid grid-cols-2 gap-3'>
        <Input
          label='Year'
          name='year'
          control={control}
          rules={{ required: 'Year is required' }}
          type='Number'
        />
        <Input
          label='Mileage'
          name='mileage'
          control={control}
          type='number'
          rules={{ required: 'Mileage is required' }}
        />
      </div>
      <Input
        label='Image Url'
        name='ImageUrl'
        control={control}
        rules={{ required: 'Image Url is required' }}
      />

      <div className='grid grid-cols-2 gap-3'>
        <Input
          label='Reserve Price (Enter 0 if no reserve)'
          name='reservePrice'
          control={control}
          rules={{ required: 'Reserve Price is required' }}
          type='number'
        />
        <DateInput
          label='Auction end Date/Time'
          name='auctionEnd'
          control={control}
          type='Date'
          dateFormat='dd MMMM yyy h:mm a'
          showTimeSelect
          rules={{ required: 'Auction end date is required' }}
        />
      </div>

      <div className='flex justify-between'>
        <Button outline color='grey'>
          Cancel
        </Button>
        <Button
          outline
          color='success'
          isProcessing={isSubmitting}
          disabled={!isValid}
          type='submit'
        >
          Submit
        </Button>
      </div>
    </form>
  );
};
