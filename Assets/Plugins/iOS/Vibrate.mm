// https://srdeveloper.tistory.com/5

#import <UIKit/UIKit.h>
#import <AudioToolBox/AudioToolBox.h>

extern "C" void Vibrate(int _n)
{
    AudioServicesPlaySystemSound(_n);
}
