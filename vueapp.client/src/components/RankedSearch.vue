<template>
  <div>
    <h1>Ranked Search</h1>
    <div class="form-group">
      <label for="search-term">Search:</label>
      <input type="text" class="form-control" id="search-term" aria-describedby="emailHelp" placeholder="Enter a phrase to search" v-model="searchTerm">
    </div>
    <div class="form-group mt-2">
      <label for="exampleInputPassword1">Rank Url:</label>
      <input type="text" class="form-control" id="rank-url" placeholder="Enter a Url to rank" v-model="rankUrl">
    </div>
    <button type="button" class="btn btn-primary mt-2" @click="fetchData">Search</button>

    <div v-if="loading">
      Loading...
    </div>

    <div v-if="post" class="mt-3">
      <p>Rank: {{post.ranks}}</p>
      <p>Links: {{post.links}}</p>
    </div>

    <div v-if="error">
      <div v-for="message in error.errors" :key="message">
        {{message[0]}}
      </div>
    </div>
  </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type RestError = {
        status: number,
        errors: {
            [key:string]: Array<string>
        }
    };

    type RankedSearchResult = {
        ranks: Array<number>,
        links: Array<string>,
    };

    interface Data {
        loading: boolean,
        result: null | RankedSearchResult,
        error: null | RestError,
        searchTerm: string,
        rankUrl: string,
    };

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                post: null,
                error: null,
                searchTerm: '',
                rankUrl: '',
            };
        },
        async created() {
        },
        methods: {
            async fetchData() {
                this.post = null;
                this.error = null;
                this.loading = true;

                const response = await fetch(`rankedsearch?searchterm=${this.searchTerm}&rankurl=${this.rankUrl}`);
                if (response.ok) {
                    this.post = await response.json();
                    console.log('success', this.post);
                }else{
                    this.error = await response.json();
                    console.log('error', this.error);
                }
                this.loading = false;
            }
        },
    });
</script>

<style scoped>

</style>
