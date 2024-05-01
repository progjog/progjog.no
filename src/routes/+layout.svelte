<script>

	import "../app.pcss";
    	import Header from './Header.svelte';
	import Footer from "./Footer.svelte";
    	import './styles.css';

</script>

<svelte:head>

	<meta name="description" content="Unlock your running potential with our web app, designed for runners of all levels. Access a wide range of free training programs, create and share your own plans, and benefit from AI-powered tips and feedback. Whether you're starting out or looking to improve, find everything you need to excel in your running journey." />

</svelte:head>

<div class="app">

	<Header></Header>

	<main>
		<slot></slot>
	</main>

	<Footer />

</div>

<style>
</style>
