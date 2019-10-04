function include(url){document.write('<script type="text/javascript" src="'+url+'"></script>')}
include('js/jquery.mousewheel.min.js');include('js/jquery.easing.js');include('js/jquery.flexslider.js');include('js/jquery.spritely.js');include('js/tmMultimediaGallery.js');if(!FJSCore.mobile){include('js/hoverIntent.js');include('js/superfish.js');include('js/spin.min.js');include('js/jquery.stellar.js');include('js/greensock/TweenMax.min.js');}
var $win=$(window),$doc=$(document),currentIndex=1,msie=(navigator.appVersion.indexOf("MSIE")!==-1);function spinnerInit(){var opts={lines:11,length:10,width:5,radius:14,corners:1,color:'#fff',speed:1.3,trail:5},spinner=new Spinner(opts).spin($('#webSiteLoader')[0]);}
function spinnerInitGallery(){var opts={lines:11,length:10,width:5,radius:14,corners:1,color:'#fff',speed:1.3,trail:5}
$('.imgSpinner').each(function(){var spinner=new Spinner(opts).spin($(this)[0]);})}
function initFullGallery(){console.log(currentIndex);$('.gallery_1 .gall_item').on('click',function(e){currentIndex=Number($(this).attr('data-gall-number'))-1;});$('.close-icon').on('click',function(){$fullGallery&&$fullGallery.trigger('hideGallery');})
$fullGallery=$(".galleryHolder");$fullGallery.tmMultimediaGallery({startIndex:currentIndex,showOnInit:true,container:'.galleryContainer',imageHolder:'.imageHolder',next:'.nextButton',prev:'.prevButton',prev:'.prevButton',pagination:'.inner',spinner:'.imgSpinner',animationSpeed:'1.2',autoPlayState:false,controlDisplay:true,paginationDisplay:true,autoPlayTime:12,alignIMG:'center',mobile:FJSCore.tablet,onShowActions:function(){setTimeout(function(){$('.controls-holder, .close-icon').addClass('show-item');$win.trigger('resize');},500);},onHideActions:function(){$('.controls-holder, .close-icon').removeClass('show-item');$('.galleryContainer').removeClass('showGallery');}});$win.trigger('resize');$('.full-btn').on('click',function(){toggleFullScreen();});}
$win.load(function(){if(!FJSCore.mobile){if(window.navigator.userAgent.indexOf("MSIE 9")===-1){$(this).on('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend',function(){$(this).css('display','none');}).addClass('hideSplash');}else{$(this).addClass('hideSplashIE');}
$win.trigger('scrollEnable','true');}})
function initPlugins(){if(!FJSCore.mobile){spinnerInit();$('#glow-inner-1').sprite({fps:30,no_of_frames:90});$('#glow-inner-2').sprite({fps:30,no_of_frames:90});$('#glow-inner-3').sprite({fps:10,no_of_frames:90});$('#glow-inner-4').sprite({fps:15,no_of_frames:90});$('#glow-inner-5').sprite({fps:15,no_of_frames:90});$('.mainNav').superfish({speed:'fast',delay:800,animation:{height:'show'},animationOut:{height:'hide'}});}
if(!FJSCore.mobile){$win.on('resize',function(){$('#content').css({'width':$win.width()*$('#content>div').length,'height':$win.height(),'overflow':'hidden'})
$('.parallax').find('>div').each(function(){var
scaleValue=$win.width()/ 1920,
scaleString='scale('+ scaleValue+','+ scaleValue+')',cssProp={'height':$win.height(),'-webkit-transform':scaleString,'-ms-transform':scaleString,'transform':scaleString};$(this).css(cssProp).find('>*').attr('data-stellar-offset-parent','true');});$('#content>div').each(function(ind){$(this).css({'width':$win.width()});})}).trigger('resize').trigger('scrollEnable','false');var $scrollObj=$('body, html');function keyboardEvent(e){if(e.keyCode==37){$scrollObj.trigger('mousewheel','-1');}
else if(e.keyCode==39){$scrollObj.trigger('mousewheel','1');}}
$doc.on('keydown',keyboardEvent);$.stellar.positionProperty.transform={setPosition:function($elem,left,startingLeft,top,startingTop){var transformString='translate3d('+(left- startingLeft)+'px, '+(top- startingTop)+'px, 0)',cssPropTranslate={'-webkit-transform':transformString,'-ms-transform':transformString,'transform':transformString};$elem.css(cssPropTranslate);}}
$.stellar({responsive:true,verticalScrolling:false,horizontalOffset:250,positionProperty:'transform',hideDistantElements:false});}}
function initPluginsForPage(){if(!msie){function historyBack(){if(FJSCore.prevState!==undefined){history.go(-1);return false;}}
$('.history-back').on('click',historyBack);}}
$(function(){$("#year").text((new Date).getFullYear());if(!FJSCore.mobile&&!FJSCore.tablet){TweenMax.from($('#home #bg-page-1'),2.0,{delay:2.7,css:{opacity:0,height:"50px",width:"50px",top:"350px",left:"1020px"},ease:Circ.easeOut})
TweenMax.from($('#home #logo1'),0.8,{delay:5.0,css:{opacity:0,top:"-50px"},ease:Expo.easeOut})
TweenMax.from($('#home .slogan1'),1.2,{delay:3.8,css:{opacity:0,rotationY:180,transformOrigin:"left 50% -100"},ease:Cubic.easeOut})
TweenMax.from($('#home #logo2'),0.3,{delay:4.5,css:{opacity:0,bottom:"-50px"},ease:Expo.easeOut})
TweenMax.from($('#home .slogan2'),1.0,{delay:4.0,css:{opacity:0,rotationY:-180,transformOrigin:"right 50% -100"},ease:Cubic.easeOut})
TweenMax.from($('#home .element_1'),0.8,{delay:3.1,css:{opacity:0,left:"-580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_2'),0.8,{delay:3.2,css:{opacity:0,left:"-580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_3'),0.8,{delay:3.3,css:{opacity:0,right:"-580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_4'),0.8,{delay:3.4,css:{opacity:0,right:"-580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_5'),0.8,{delay:3.5,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_6'),0.8,{delay:3.3,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_7'),0.8,{delay:3.4,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_8'),0.8,{delay:3.5,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_9'),0.8,{delay:3.1,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_10'),0.8,{delay:3.2,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_11'),0.8,{delay:3.4,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_12'),0.8,{delay:3.5,css:{opacity:0,left:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_13'),0.8,{delay:3.6,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_14'),0.8,{delay:3.7,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_15'),0.8,{delay:3.8,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_16'),0.8,{delay:3.9,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#home .element_17'),0.8,{delay:4.0,css:{opacity:0,right:"580px",scale:0.2},ease:Expo.easeOut})
TweenMax.from($('#mouse-hint'),2.0,{delay:4.7,css:{opacity:0,bottom:"-100px"},ease:Expo.easeOut})
TweenMax.from($('footer'),2.0,{delay:3.7,css:{opacity:0,bottom:"-100px"},ease:Expo.easeOut})}
if(FJSCore.mobile){$('body').css({'min-width':'inherit'});$(document).on('show','#mobile-content>*',function(e,d){$(this).addClass('active');initPlugins();initFullGallery();}).on('hide','#mobile-content>*',function(e,d){$(this).removeClass('active');});}
initPlugins();var $pages=$('#other_pages'),$body=$('body');$pages.on('show','>*',function(e,d){$.when(d.elements).then(function(){$body.trigger('resizeContent');$pages.addClass('showPages');initPluginsForPage();initFullGallery();d.curr.addClass('active').stop().css({'display':'block','top':-$(window).outerHeight()*1.5}).animate({'top':0},{duration:800,complete:function(){$body.trigger('resizeContent');}})})}).on('hide','>*',function(e,d){$pages.removeClass('showPages');$(this).removeClass('active').stop().animate({'top':-$(window).outerHeight()*1.5},{duration:400,complete:function(){$(this).css({'display':'none'});setTimeout(function(){$body.trigger('resizeContent');},0);}})})})
$(window).load(function(){$("#webSiteLoader").fadeOut(500,0,function(){if(!FJSCore.state.match(new RegExp(/(.html|.php)/g))){$('#splash').show(0).attr('href','./'+ FJSCore.state);}else{$('#splash').hide(0);}
$("#webSiteLoader").remove();});$('.flexslider').flexslider({animationSpeed:1000,animation:"fade",slideshow:false,controlNav:false,prevText:'',nextText:''});FJSCore.modules.responsiveContainer({type:'inner',container:'#other_pages',elementsSelector:'#other_pages>div',defStates:'',affectSelectors:'',activePageSelector:'.active'});if(FJSCore.mobile){$('#mobile-header>*').wrapAll('<div class="container"></div>');$('#mobile-footer>*').wrapAll('<div class="container"></div>');}
$(this).trigger('afterload');});