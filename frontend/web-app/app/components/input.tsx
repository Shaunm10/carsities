import React from 'react';
import { UseControllerProps, useController } from 'react-hook-form';

type Props = {
  label: string;
  type?: string;
  showLabel: boolean;
} & UseControllerProps;

export const input = (props: Props) => {
  const { fieldState, field } = useController({ ...props, defaultValue: '' });
  return <div className='mb-3'>input</div>;
};
