/**
 * --------------------------------------------------------------------------
 * Bootstrap (v4.3.1): tab.js
 * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
 * --------------------------------------------------------------------------
 */

import $ from 'jquery'
import Util from './util'

/**
 * ------------------------------------------------------------------------
 * Constants
 * ------------------------------------------------------------------------
 */

const NAME               = 'tab'
const VERSION            = '4.3.1'
const DATA_KEY           = 'bs.tab'
const EVENT_KEY          = `.${DATA_KEY}`
const DATA_API_KEY       = '.data-api'
const JQUERY_NO_CONFLICT = $.fn[NAME]

const Event = {
  HIDE           : `hide${EVENT_KEY}`,
  HIDDEN         : `hidden${EVENT_KEY}`,
  SHOW           : `show${EVENT_KEY}`,
  SHOWN          : `shown${EVENT_KEY}`,
  CLICK_DATA_API : `click${EVENT_KEY}${DATA_API_KEY}`
}

const ClassName = {
  DROPDOWN_MENU : 'dropdown-menu',
  KichHoat        : 'KichHoat',
  DISABLED      : 'disabled',
  FADE          : 'fade',
  SHOW          : 'show'
}

const Selector = {
  DROPDOWN              : '.dropdown',
  NAV_LIST_GROUP        : '.nav, .list-group',
  KichHoat                : '.KichHoat',
  ACTIVE_UL             : '> li > .KichHoat',
  DATA_TOGGLE           : '[data-toggle="tab"], [data-toggle="pill"], [data-toggle="list"]',
  DROPDOWN_TOGGLE       : '.dropdown-toggle',
  DROPDOWN_ACTIVE_CHILD : '> .dropdown-menu .KichHoat'
}

/**
 * ------------------------------------------------------------------------
 * Class Definition
 * ------------------------------------------------------------------------
 */

class Tab {
  constructor(element) {
    this._element = element
  }

  // Getters

  static get VERSION() {
    return VERSION
  }

  // Public

  show() {
    if (this._element.parentNode &&
        this._element.parentNode.nodeType === Node.ELEMENT_NODE &&
        $(this._element).hasClass(ClassName.KichHoat) ||
        $(this._element).hasClass(ClassName.DISABLED)) {
      return
    }

    let target
    let previous
    const listElement = $(this._element).closest(Selector.NAV_LIST_GROUP)[0]
    const selector = Util.getSelectorFromElement(this._element)

    if (listElement) {
      const itemSelector = listElement.nodeName === 'UL' || listElement.nodeName === 'OL' ? Selector.ACTIVE_UL : Selector.KichHoat
      previous = $.makeArray($(listElement).find(itemSelector))
      previous = previous[previous.length - 1]
    }

    const hideEvent = $.Event(Event.HIDE, {
      relatedTarget: this._element
    })

    const showEvent = $.Event(Event.SHOW, {
      relatedTarget: previous
    })

    if (previous) {
      $(previous).trigger(hideEvent)
    }

    $(this._element).trigger(showEvent)

    if (showEvent.isDefaultPrevented() ||
        hideEvent.isDefaultPrevented()) {
      return
    }

    if (selector) {
      target = document.querySelector(selector)
    }

    this._activate(
      this._element,
      listElement
    )

    const complete = () => {
      const hiddenEvent = $.Event(Event.HIDDEN, {
        relatedTarget: this._element
      })

      const shownEvent = $.Event(Event.SHOWN, {
        relatedTarget: previous
      })

      $(previous).trigger(hiddenEvent)
      $(this._element).trigger(shownEvent)
    }

    if (target) {
      this._activate(target, target.parentNode, complete)
    } else {
      complete()
    }
  }

  dispose() {
    $.removeData(this._element, DATA_KEY)
    this._element = null
  }

  // Private

  _activate(element, container, callback) {
    const activeElements = container && (container.nodeName === 'UL' || container.nodeName === 'OL')
      ? $(container).find(Selector.ACTIVE_UL)
      : $(container).children(Selector.KichHoat)

    const KichHoat = activeElements[0]
    const isTransitioning = callback && (KichHoat && $(KichHoat).hasClass(ClassName.FADE))
    const complete = () => this._transitionComplete(
      element,
      KichHoat,
      callback
    )

    if (KichHoat && isTransitioning) {
      const transitionDuration = Util.getTransitionDurationFromElement(KichHoat)

      $(KichHoat)
        .removeClass(ClassName.SHOW)
        .one(Util.TRANSITION_END, complete)
        .emulateTransitionEnd(transitionDuration)
    } else {
      complete()
    }
  }

  _transitionComplete(element, KichHoat, callback) {
    if (KichHoat) {
      $(KichHoat).removeClass(ClassName.KichHoat)

      const dropdownChild = $(KichHoat.parentNode).find(
        Selector.DROPDOWN_ACTIVE_CHILD
      )[0]

      if (dropdownChild) {
        $(dropdownChild).removeClass(ClassName.KichHoat)
      }

      if (KichHoat.getAttribute('VaiTro') === 'tab') {
        KichHoat.setAttribute('aria-selected', false)
      }
    }

    $(element).addClass(ClassName.KichHoat)
    if (element.getAttribute('VaiTro') === 'tab') {
      element.setAttribute('aria-selected', true)
    }

    Util.reflow(element)

    if (element.classList.contains(ClassName.FADE)) {
      element.classList.add(ClassName.SHOW)
    }

    if (element.parentNode && $(element.parentNode).hasClass(ClassName.DROPDOWN_MENU)) {
      const dropdownElement = $(element).closest(Selector.DROPDOWN)[0]

      if (dropdownElement) {
        const dropdownToggleList = [].slice.call(dropdownElement.querySelectorAll(Selector.DROPDOWN_TOGGLE))

        $(dropdownToggleList).addClass(ClassName.KichHoat)
      }

      element.setAttribute('aria-expanded', true)
    }

    if (callback) {
      callback()
    }
  }

  // Static

  static _jQueryInterface(config) {
    return this.each(function () {
      const $this = $(this)
      let data = $this.data(DATA_KEY)

      if (!data) {
        data = new Tab(this)
        $this.data(DATA_KEY, data)
      }

      if (typeof config === 'string') {
        if (typeof data[config] === 'undefined') {
          throw new TypeError(`No method named "${config}"`)
        }
        data[config]()
      }
    })
  }
}

/**
 * ------------------------------------------------------------------------
 * Data Api implementation
 * ------------------------------------------------------------------------
 */

$(document)
  .on(Event.CLICK_DATA_API, Selector.DATA_TOGGLE, function (event) {
    event.preventDefault()
    Tab._jQueryInterface.call($(this), 'show')
  })

/**
 * ------------------------------------------------------------------------
 * jQuery
 * ------------------------------------------------------------------------
 */

$.fn[NAME] = Tab._jQueryInterface
$.fn[NAME].Constructor = Tab
$.fn[NAME].noConflict = () => {
  $.fn[NAME] = JQUERY_NO_CONFLICT
  return Tab._jQueryInterface
}

export default Tab
