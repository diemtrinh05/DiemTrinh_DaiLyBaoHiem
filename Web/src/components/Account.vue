<template>
    <div class="mx-auto" style="max-width: 20rem;">
        <div v-if="!auth.isAuthenticated()">
            <h2>Log in to your account</h2>
            <b-form>
                <b-form-group id="usernameGroup"
                              label="Username:"
                              label-for="userName">
                    <b-form-input id="userName"
                                  type="text"
                                  v-model="credentials.username"
                                  required
                                  placeholder="Enter username">
                    </b-form-input>
                </b-form-group>

                <b-form-group id="passwordGroup"
                              label="Password:"
                              label-for="password">
                    <b-form-input id="password"
                                  type="password"
                                  v-model="credentials.password"
                                  required
                                  placeholder="Enter password">
                    </b-form-input>
                </b-form-group>

                <b-button type="button" v-on:click="login()" variant="primary">Login</b-button>
            </b-form>
            <p v-if="error" class="text-danger mt-3">{{ error }}</p>

        </div>
        <div v-else>
            <b-form>
                <b-button type="button" v-on:click="logout()" variant="primary">Logout</b-button>
            </b-form>
        </div>
    </div>

</template>

<script>
    import auth from './http/Auth'

    export default {
        name: "Account",
        data() {
            return {
                credentials: {
                    username: '',
                    password: ''
                },
                error: '',
                auth: auth
            }
        },
        methods: {
            login() {
                this.error = '';
                const credentials = {
                    login: this.credentials.username,
                    password: this.credentials.password
                };
                auth.login(this, credentials).then(() => {
                    window.location.href = '/';
                }).catch(() => {
                    this.error = 'Đăng nhập thất bại. Kiểm tra lại tài khoản, mật khẩu hoặc URL service.';
                });
            },
            logout() {
                auth.logout();
                window.location.reload();
            }
        }

    }
</script>
