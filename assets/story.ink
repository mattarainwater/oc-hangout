CLEAR

SET_BACKGROUND #Texture:backgrounds/background-1.png
SPAWN_ACTOR #Id:1 #Sprite:kathegras.tres #Position:250,103 #DefaultAnimationName:idle #Flip:true #ZIndex:3

FADE_IN

PAUSE #Time:.5

PLAY_ANIMATION #Id:1 #AnimationName:talking #Flip:true #Halting:false

This's the place, it is. #Name:Kathegras #PortraitSprite:kathegras-portrait.png #Index:0
Doesn't quite match the description, does it? #Name:Kathegras #PortraitSprite:kathegras-portrait.png #Index:0
'A foul house - a gathering of devils, fiends, and other horned miscreants'. #Name:Kathegras #PortraitSprite:kathegras-portrait.png #Index:0
My granny's grave, this place hasn't seen evil in years. #Name:Kathegras #PortraitSprite:kathegras-portrait.png #Index:1

HIDE_DIALOGUE
PLAY_ANIMATION #Id:1 #AnimationName:idle #Flip:true #Halting:false
PAUSE #Time:.5
PLAY_ANIMATION #Id:1 #AnimationName:talking #Flip:true #Halting:false

Ah, I'll have me a gander about - spread some sage, mayhap. #Name:Kathegras #PortraitSprite:kathegras-portrait.png #Index:0
That'll be good enough to get paid. #Name:Kathegras #PortraitSprite:kathegras-portrait.png #Index:1

PLAY_ANIMATION #Id:1 #AnimationName:idle #Flip:true #Halting:false

FADE_OUT

CLEAR

PAUSE #Time:.5

REMOVE_BACKGROUND

PAUSE #Time:.5

SET_BACKGROUND #Texture:backgrounds/background-2.png #ZIndex:0
SPAWN_ACTOR #Id:99 #Sprite:bar.tres #Position:20,110 #DefaultAnimationName:idle #Flip:false #ZIndex:2

MOVE_ACTOR #Id:1 #To:265,98 #AnimationName:idle #TimeToMove:0 #Halting:false #Flip:true #EndAnimation:idle
SPAWN_ACTOR #Id:2 #Sprite:endra.tres #Position:45,98 #DefaultAnimationName:idle #Flip:false #ZIndex:3
SPAWN_ACTOR #Id:3 #Sprite:adriane.tres #Position:15,98 #DefaultAnimationName:idle #Flip:false #ZIndex:3
SPAWN_ACTOR #Id:4 #Sprite:lycoris.tres #Position:105,98 #DefaultAnimationName:idle #Flip:false #ZIndex:3
SPAWN_ACTOR #Id:5 #Sprite:veo.tres #Position:150,98 #DefaultAnimationName:idle #Flip:true #ZIndex:3
SPAWN_ACTOR #Id:6 #Sprite:bugger.tres #Position:60,85 #DefaultAnimationName:idle #Flip:false #ZIndex:1

FADE_IN

PAUSE #Time:.5

PLAY_ANIMATION #Id:5 #AnimationName:talking #Flip:true #Halting:false

I call to order this meeting of Demons, Dragons, and Other Horn- #Name:Veo #PortraitSprite:veo-portrait.png #Index:0

PLAY_ANIMATION #Id:5 #AnimationName:idle #Flip:true #Halting:false
PLAY_ANIMATION #Id:1 #AnimationName:scare #Flip:true #Halting:false
PLAY_ANIMATION #Id:4 #AnimationName:point #Flip:false #Halting:false

Get out! No horns, no entry! #Name:Lycoris #PortraitSprite:lycoris-portrait.png #Index:1

PLAY_ANIMATION #Id:4 #AnimationName:idle #Flip:false #Halting:false
PLAY_ANIMATION #Id:5 #AnimationName:sword-talking #Flip:false #Halting:false

Who are you? #Name:Veo #PortraitSprite:veo-portrait.png #Index:1

PLAY_ANIMATION #Id:5 #AnimationName:sword-pose #Flip:false #Halting:false

Er- #Name:Kathegras #PortraitSprite:kathegras-portrait.png #Index:2

I was told this would be a private affair. #Name:Endra #PortraitSprite:endra-portrait.png #Index:0

PLAY_ANIMATION #Id:2 #AnimationName:sword-talking #Flip:false #Halting:false

Yeah, get her, sis! I want her hat! #Name:Kaixus #PortraitSprite:kaixus-portrait.png #Index:0

PLAY_ANIMATION #Id:2 #AnimationName:sword-pose #Flip:false #Halting:false
PLAY_ANIMATION #Id:3 #AnimationName:talking #Flip:false #Halting:false

Oh, we can let *one* hornless stay, surely? #Name:Adriane #PortraitSprite:adriane-portrait.png #Index:0

PLAY_ANIMATION #Id:3 #AnimationName:idle #Flip:false #Halting:false
PLAY_ANIMATION #Id:6 #AnimationName:talking #Flip:false #Halting:false

Better idea! #Name:Bugger #PortraitSprite:bugger-portrait.png #Index:0

Someone get me a hammer, some nails, break a leg off that stool... #Name:Bugger #PortraitSprite:bugger-portrait.png #Index:0

We'll give her her own horn! Then she can stay! #Name:Bugger #PortraitSprite:bugger-portrait.png #Index:0

PLAY_ANIMATION #Id:6 #AnimationName:idle #Flip:false #Halting:false

I'll, uh, I'll go, yeah? #Name:Kathegras #PortraitSprite:kathegras-portrait.png #Index:2

HIDE_DIALOGUE
DESPAWN_ACTOR #Id:1
PAUSE #Time:.5

PLAY_ANIMATION #Id:3 #AnimationName:wave #Flip:false #Halting:false

Bye! #Name:Adriane #PortraitSprite:adriane-portrait.png #Index:1

FADE_OUT

CLEAR

-> END