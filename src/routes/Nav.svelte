<script>

	import { page } from "$app/stores";
	import { Navbar, NavBrand, NavLi, NavUl, NavHamburger, Button } from 'flowbite-svelte'

	$: activeUrl = $page.url.pathname;
  	let activeClass = "text-white bg-green-700 md:bg-transparent md:text-green-700 md:dark:text-white dark:bg-green-600 md:dark:bg-transparent";
  	let nonActiveClass = "text-gray-700 hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-green-700 dark:text-gray-400 md:dark:hover:text-white dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent";
	
</script>

<div class="relative">

	<Navbar  >

	  <NavBrand href="/">

	    <!--<img src="/images/flowbite-svelte-icon-logo.svg" class="me-3 h-6 sm:h-9" alt="Flowbite Logo" />-->

	    <span class="self-center whitespace-nowrap text-xl font-semibold dark:text-white">Progjog</span>

	  </NavBrand>

	  <div class="flex md:order-2">

	  	<!--<Button size="sm">Open App</Button>-->

	    <NavHamburger />

	  </div>

	  <NavUl {activeUrl} {activeClass} {nonActiveClass} class="order-1" >

	  <!--

	    <NavLi href="/">Home</NavLi>

	    <NavLi href="/about">About</NavLi>

	  -->
	  </NavUl>


	</Navbar>

</div>
