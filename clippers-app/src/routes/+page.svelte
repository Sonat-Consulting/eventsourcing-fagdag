<script>
    import { onMount } from "svelte";
	import { getQueueView } from "../services/projection";
	import { startHaircut, completeHaircut } from "../services/eventstore";
	import { invalidateAll } from '$app/navigation';
	
	let data = [];
	$: queue = data;
	onMount(async () => {
    	const res = await getQueueView();
		data = res;
  });

	async function doStart (haircutId) {
		try {
			console.log("START clicked." + haircutId);
			await startHaircut(haircutId, '11111');

		} catch (error) {
			console.error(error);
		}
	}

	async function doComplete (haircutId) {
		try {
			console.log("COMPLETE clicked." + haircutId);
			await completeHaircut(haircutId);

		} catch (error) {
			console.error(error);
		}
	}

	function reloadAll(){
		invalidateAll();
	};
	//export let name;
</script>

<main>
	<section>
	<h3>Clippers Super UI ++</h3>
	</section>
	<section>
		<button on:click={invalidateAll}>Reload</button>
	</section>
	<section>
	<table>
        <th><b>Navn</b></th>
        <th><b>Status</b></th>
        {#each queue as view}
            <tr>	
                <td>{view.DisplayName}</td>  
                <td>{view.Status}</td>
				<td><button on:click={doStart(view.HaircutId)}>Start</button></td>
				<td><button on:click={doComplete(view.HaircutId)}>Ferdig</button></td>
				<td></td>
            </tr>	
        {/each}
        </table>
	</section>
</main>

<style>
	main {
		text-align: left;
		padding: 1em;
		max-width: 240px;
		margin: 0 auto;
	}

	h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}

	h3 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 2em;
		font-weight: 100;
	}

	th {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 1em;
		font-weight: 100;
	}

	td {
		font-size: 1em;
		font-weight: 100;
	}


	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>