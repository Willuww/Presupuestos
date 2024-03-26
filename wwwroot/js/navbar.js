import { Dropdown } from 'flowbite';
import type { DropdownOptions, DropdownInterface } from 'flowbite';
import type { InstanceOptions } from 'flowbite';

// set the dropdown menu element
const $targetEl: HTMLElement = document.getElementById('dropdownMenu');

// set the element that trigger the dropdown menu on click
const $triggerEl: HTMLElement = document.getElementById('dropdownButton');

// options with default values
const options: DropdownOptions = {
    placement: 'bottom',
    triggerType: 'click',
    offsetSkidding: 0,
    offsetDistance: 10,
    delay: 300,
    onHide: () => {
        console.log('dropdown has been hidden');
    },
    onShow: () => {
        console.log('dropdown has been shown');
    },
    onToggle: () => {
        console.log('dropdown has been toggled');
    },
};

// instance options object
const instanceOptions: InstanceOptions = {
    id: 'dropdownMenu',
    override: true
};

/*
 * targetEl: required
 * triggerEl: required
 * options: optional
 * instanceOptions: optional
 */
const dropdown: DropdownInterface = new Dropdown(
    $targetEl,
    $triggerEl,
    options,
    instanceOptions
);

// show the dropdown
dropdown.show();