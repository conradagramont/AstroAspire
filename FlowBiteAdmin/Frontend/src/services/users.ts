/* eslint-disable no-param-reassign */

/*
import { faker } from '@faker-js/faker';
import { RANDOMIZE } from '../app/constants.js';
import type { Users } from '../types/entities.js';

import usersStaticJSON from '../../data/users.json' assert { type: 'json' };

const usersStaticData: Users = usersStaticJSON;

export function getUsers(randomize = RANDOMIZE) {
	console.log('getUsers');

	const result = randomize
		? usersStaticData.map((p) => {
				p.name = faker.person.fullName();
				p.email = faker.internet.email();
				p.position = faker.person.jobType();
				p.country = faker.location.country();
				return p;
		  })
		: usersStaticData;

	return result;
}
*/