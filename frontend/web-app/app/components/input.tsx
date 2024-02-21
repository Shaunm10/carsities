import { Label, TextInput } from 'flowbite-react';
import React, { HTMLInputTypeAttribute } from 'react';
import { UseControllerProps, useController } from 'react-hook-form';

type Props = {
  label: string;
  type?: HTMLInputTypeAttribute;
  showLabel?: boolean;
} & UseControllerProps;

const Input = (props: Props) => {
  /** Allows us to create custom inputs for react-hook-form */
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
