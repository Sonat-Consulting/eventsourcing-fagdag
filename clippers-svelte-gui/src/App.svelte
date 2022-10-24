<script>
    import { onMount, onDestroy } from "svelte";
	import { getQueueView } from "./projection";
	import { createHaircut, startHaircut, completeHaircut } from "./eventstore";
	import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
	import IoIosHourglass from 'svelte-icons/io/IoIosHourglass.svelte'
	import TiScissors from 'svelte-icons/ti/TiScissors.svelte'
	let fornavnListe = ["Markus", "Lilly", "Emma", "Noa", "Markus", "Amanda", "Maja", "Vilde", "Nicolai", "Sarah", "Phillip", "Sophie", "Mathilde", "Anna", "Casper", "Astri", "Elias", "Johan", "Noah", "Axel", "Maria", "Johannes", "Iben", "Jonas", "Agnes", "Nora", "Sigrid", "Kasper", "Emma", "Adam", "Astri", "Anna", "Johann", "Viktoria", "Oskar", "Jakob", "Sophie", "Elias", "Kasper", "Theo", "Hanna", "Aleksander", "Oline", "Lea", "Oline", "Ida", "Hannah", "Sigrid", "Ellinor", "Aleksander", "Olav", "Sebastian", "Ellinor", "Kasper", "Astrid", "Bantam", "Haakon", "Jonas", "Liam", "Jacob", "Kaia", "Emma", "Tiril", "Victor", "Håkon", "Victoria", "Felix", "Amelia", "Sophia", "Liam", "Selma", "Herman", "Viktoria", "Johan", "Aegon", "Marie", "Emilie", "Henry", "Emil", "Mathilde", "Eline", "Noah", "Dany", "Matilde", "Amanda", "Ella", "Fredeico", "Mikkel", "Even", "Jonas", "Astri", "Mikaela", "Philip", "Jonas", "Jonna", "Sophie", "Lilly", "Oliver", "Alexander", "Agnes"];

	let data = [];
	$: queue = data;
	let displayName = createRandomFornavn();
	let connection;
	
	function createRandomFornavn () {
		let number = Math.floor(Math.random() * fornavnListe.length);
		console.log("Random number: " + number);
		return fornavnListe[number];
	};

	onMount(async () => {
		const res = await getQueueView();
		data = res;
		//const apiInterval = setInterval(async () => {
		//	const res = await getQueueView();
		//	data = res;
		//}, 5000); 
		const connect = new HubConnectionBuilder()
      		.withUrl("https://localhost:7247/projectionchanged")
      		.withAutomaticReconnect()
      		.build();
		connection = connect;
		if (connection) {
			connection
				.start()
				.then(() => {
				connection.on("SendNotification", (message) => {
					//console.log("projectionchanged TRIGGERED.");
					reloadWrapper();
				});
				})
				.catch((error) => console.log(error));
			}

  	});

	const reloadWrapper = async function(){
		//console.log("reloadWrapper Called.");
		await reload();
	}

	async function reload(){
		//console.log("reload Called.");
		const res = await getQueueView();
		data = res;
	};
  	
	onDestroy(() => {
    	clearInterval(apiInterval);
	});


	
	async function doCreate () {
		try {
			//console.log("Create clicked." + displayName);
			await createHaircut(displayName);
			displayName = createRandomFornavn();
			//setTimeout(() => {  reload(); }, 500);
		} catch (error) {
			console.error(error);
		}
	}

	async function doStart (haircutId) {
		try {
			//console.log("START clicked." + haircutId);
			await startHaircut(haircutId, '11111');
			//setTimeout(() => {  reload(); }, 500);

		} catch (error) {
			console.error(error);
		}
	}

	async function doComplete (haircutId) {
		try {
			//console.log("COMPLETE clicked." + haircutId);
			await completeHaircut(haircutId);
			//setTimeout(() => {  reload(); }, 500);

		} catch (error) {
			console.error(error);
		}
	}

	
	//export let name;
</script>

<main>
	<section>
		<img src="./../images/AniClippers.gif" alt="background" />
		<div class="ruler"></div>
	</section>
	<section>
		<input value={displayName}/>
		<button on:click={doCreate}>Tryll ny kunde</button>
		<div class="ruler"></div>
	</section>
	<section>
	<table>
        {#each queue as view}
            <tr>	
                <td class="customer">{view.DisplayName}</td> 
				<td>
					{#if view.Status!=='waiting'}
						<div class="icon">
							<TiScissors />
						</div>
					{:else if view.Status!=='serving'}
						<div class="icon">
							<IoIosHourglass />
						</div>
					{/if}
				</td> 
                
				<td>
					<button  on:click={doStart(view.HaircutId)} disabled={view.Status!=='waiting'}>Start</button>
				</td>
				<td>
					<button on:click={doComplete(view.HaircutId)} disabled={view.Status!=='serving'}>Ferdig</button>
				</td>
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

	td {
		
		margin: 0 0 1em 0;
		font-size: 1em;
		font-weight: 100;

	}
	
	.customer {
        padding: 0 1em 0 0;
		font-size: 2em;
		width: 120px;
	}

	.icon {
    	color: grey;
    	width: 32px;
    	height: 32px;
		margin: 0 10px 0 0;
	}
	.ruler {
		width: 310px;
		border: 1px solid #aaa;
		border-radius: 2px;
		margin: 1em 0 1em 0;
		border-color: #ff3e00;
	}
    img {
		width: 300px;
	}

	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>