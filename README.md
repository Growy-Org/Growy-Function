## How to run

cd Growy.Function
func start

## To Implement
- Validation on Parent and Child Edit wish so that it only apply to certain field
- Authorisation so that only current home Id info can be accessed by the tenant 
- Authentication

## Future Feature
- Icon & avatar
- Repeat Assignment Scheduler
- Remove home, child, parent are not allowed when there is linked resource
- Home Analytics. (Get All Wishes, Achievement, etc..)
- Make Error throw

### Game Category Profile & Mini Game

Inference
Creativity
Observation
Spacial
Memory
Calculation

## UI

Fun fact about this DOB (some chinese tradition), zodiac
Some management

## Tech

- Authentication
- Authorisation to only modify resource from the same tenant (home)
- Pagination (now returning all assignment)
- Http version 
- Request validation
- Transactional Query in editing multiple table
- Preventing Negative Points from a child
- Testing and Unittest
- Handle different sql exception via middle ware 
  - Duplicate Step Order
  - DB not responding etc
- Async Analytic
- change Fulfill typo
## Some analytics

What matters to kids these days at different age?
What makes parent proud ? Most commonly achievement set by different parent at different age

## DB setup in another project

Growy DB
In the future, image can be published and pull down for integration test

## Mission

- Would it not be great to Track on your kids growth? collection of memory
- Only Recruit Parent who loves kids and share the same vision

## Authentication
- Configure Authentication at Microsoft Entra
- Then Added it in Function > Authentication
- Principal id can be accessed with header `X-MS-CLIENT-PRINCIPAL-ID`
- App also need to provide `X-HOME-ID` so that it could match with the Home Id found using IdpId

## Endpoint convention
```
GET SINGLE : GET https://domain/resource/<id>
GET ALL: GET https://domain/<id>/resources
ADD: POST https://domain/<home-id>/resource
EDIT : PUT https://domain/resource {payload}
DELETE : DELETE https://domain/resource/<id>
```
