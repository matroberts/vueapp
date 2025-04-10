<template>
    <div class="weather-component">
        <h1>Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>

        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="post" class="content">
            <table>
                <thead>
                    <tr>
                        <th>Rank</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="num in post" :key="forecast.date">
                        <td>{{ num }}</td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div v-if="error">
          <div v-for="message in error.errors" :key="error.errors.key">
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

    interface Data {
        loading: boolean,
        post: null | Array<number>,
        error: null | RestError,
    };

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                post: null,
                error: null,
            };
        },
        async created() {
            // fetch the data when the view is created and the data is
            // already being observed
            await this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            async fetchData() {
                this.post = null;
                this.error = null;
                this.loading = true;

                var response = await fetch('rankedsearch');
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
th {
    font-weight: bold;
}

th, td {
    padding-left: .5rem;
    padding-right: .5rem;
}

.weather-component {
    text-align: center;
}

table {
    margin-left: auto;
    margin-right: auto;
}
</style>
