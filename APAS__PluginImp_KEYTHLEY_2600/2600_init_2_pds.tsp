# 将2600配置为双路PD电流模式
beeper.enable=0 #禁用蜂鸣器
display.screen=2 #同时显示A、B通道信息

# 配置通道A
smua.reset()
smua.source.func=smua.OUTPUT_DCVOLTS
display.smua.measure.func=display.MEASURE_DCAMPS
smua.source.limitv=1.5
smua.source.limiti=0.1
smua.source.levelv=1
smua.source.autorangei=1
smua.source.autorangev=1
smua.measure.autorangei=1
smua.measure.autorangev=1
waitcomplete()

# 配置通道B
smub.reset()
smub.source.func=smub.OUTPUT_DCVOLTS
display.smub.measure.func=display.MEASURE_DCAMPS
smub.source.limitv=1.5
smub.source.limiti=0.1
smub.source.levelv=0.5
smub.source.autorangei=1
smub.source.autorangev=1
smub.measure.autorangei=1
smub.measure.autorangev=1
waitcomplete()