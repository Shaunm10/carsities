import { Label, TextInput } from 'flowbite-react';
import React from 'react';
import { UseControllerProps, useController } from 'react-hook-form';

type Props = {
  label: string;
  type?: string;
  showLabel?: boolean;
} & UseControllerProps;

const Input = (props: Props) => {
  const { fieldState, field } = useController({ ...props, defaultValue: '' });
  const color = fieldState.error
    ? 'failure'
    : !fieldState.isDirty
    ? ''
    : 'success';
  return (
    <div className='mb-3'>
      {props.showLabel && (
        <div className='mb-2 block'>
          <Label htmlFor={field.name} value={props.label}></Label>
        </div>
      )}

      <TextInput
        {...props}
        {...field}
        placeholder={props.label}
        type={props.type || 'text'}
        color={color}
        helperText={fieldState.error?.message}
      />
    </div>
  );
};

export default Input;
