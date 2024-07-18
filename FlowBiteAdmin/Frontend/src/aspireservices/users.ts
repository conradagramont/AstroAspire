
import OpenTelemetry from '@opentelemetry/api';
import type { Users } from '../types/entities.js';


const tracer = OpenTelemetry.trace.getTracer('FlowBiteAdmin.Frontend');

const apiService = process.env['services__api__http__0'] || "";  // This value won't work, but added to suppress an error/warning that the variable is not defined
const usersEndpoint = `${apiService}/user`;

export async function getUsers() {
    const span = tracer.startSpan('Astro AspireServices - Users.ts - getUsers');
    span.setAttribute('service', 'Astro AspireServices');
    span.setAttribute('usersEndpoint', usersEndpoint);
	console.log('aspireservices/users.ts getUsers()');
	console.log('getUsers');
	console.log('usersEndpoint',usersEndpoint);

    const response = await fetch(usersEndpoint);

    const users: Users = await response.json();

    console.log('users', users);

    span.end();

	return users;
}