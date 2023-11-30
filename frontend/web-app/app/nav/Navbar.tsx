'use client';
import React from 'react';

export default function Navbar() {
	console.log('client component');
	return (
		<header className="sticky top-0 z-50 flex justify-between">
			<div>left</div>
			<div>middle</div>
			<div>right</div>
		</header>
	);
}
