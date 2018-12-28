import { createAsyncAction } from 'typesafe-actions';
import { IdentityActionTypes, IdentityPayload } from './types';
import { BranchError } from '../../common/types';
import { XboxLiveIdentity } from '../types';

export const fetchIdentity = createAsyncAction(
	IdentityActionTypes.FETCH_IDENTITY,
	IdentityActionTypes.FETCHED_IDENTITY_SUCCESS,
	IdentityActionTypes.FETCHED_IDENTITY_FAILURE,
)<IdentityPayload, XboxLiveIdentity, BranchError>();
