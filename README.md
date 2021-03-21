[![.NET 5.0](https://github.com/aimenux/HealthChecksDemo/actions/workflows/ci.yml/badge.svg)](https://github.com/aimenux/HealthChecksDemo/actions/workflows/ci.yml)

# HealthChecksDemo
```
Exploring healthchecks capabilities in web api net 5
```
>
> In this repo, i m providing multiple ways of adding healthchecks support to web api net 5
>
> :writing_hand: `Example01`
>
>> 2 uri checkers
>>
>> 1 endpoint accessible on `/`
>
> :writing_hand: `Example02`
>
>> 2 custom checkers
>>
>> 1 endpoint accessible on `/`
>
> :writing_hand: `Example03`
>
>> 3 custom checkers
>>
>> 4 endpoints accessibles on `/options1`, `/options2`, `/options3`, `/options4`
>
> :writing_hand: `Example04`
>
>> 5 custom checkers
>>
>> 1 endpoint accessible on `/healthchecks`
>>
>> ui (fluent config, inmemory storage) served on `/healthchecks-ui`
>
> :writing_hand: `Example05`
>
>> 5 custom checkers
>>
>> 2 endpoints accessibles on `/healthchecks/live`, `/healthchecks/ready`
>>
>> ui (fluent config, inmemory storage) served on `/healthchecks-ui`
>
> :writing_hand: `Example06`
>
>> 5 custom checkers
>>
>> 2 endpoints accessibles on `/healthchecks/live`, `/healthchecks/ready`
>>
>> ui (json config, absolute uris, inmemory storage) served on `/healthchecks-ui`
>
> :writing_hand: `Example07`
>
>> 5 custom checkers
>>
>> 2 endpoints accessibles on `/healthchecks/live`, `/healthchecks/ready`
>>
>> ui (json config, relative uris, inmemory storage) served on `/healthchecks-ui`
>
> :writing_hand: `Example08`
>
>> 5 custom checkers
>>
>> 2 endpoints accessibles on `/healthchecks/live`, `/healthchecks/ready`
>>
>> ui (json config, relative uris, sqllite storage) served on `/healthchecks-ui`
>
> :writing_hand: `Example09`
>
>> 5 custom checkers
>>
>> 2 endpoints accessibles on `/healthchecks/live`, `/healthchecks/ready`
>>
>> ui (json config, relative uris, sqlserver storage) served on `/healthchecks-ui`
>
> :writing_hand: `Example10`
>
>> 2 custom checkers
>>
>> 2 sqlserver checkers
>>
>> 2 endpoints accessibles on `/healthchecks/live`, `/healthchecks/ready`
>>
>> ui (json config, relative uris, sqlserver storage) served on `/healthchecks-ui`
>
> ![Example05Screen](Screenshots/Example05.png)

**`Tools`** : vs19, webapi, net 5.0, serilog